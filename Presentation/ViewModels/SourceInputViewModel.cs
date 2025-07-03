using CKL_Studio.Presentation.Commands;
using CKL_Studio.Presentation.Services.Navigation;
using CKL_Studio.Presentation.ViewModels.Base;
using CKLLib;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CKL_Studio.Presentation.ViewModels
{
    public class SourceInputViewModel : ViewModelBase, IParameterReceiver<CKL>
    {
        private readonly INavigationService _navigationService;
        private CKL _ckl;
        private Pair? _selectedPair;
        private int _dim = 1;
        private int _rowCount = 1;

        public CKL CKL
        {
            get => _ckl;
            set => SetField(ref _ckl, value);
        }

        public Pair? SelectedPair
        {
            get => _selectedPair;
            set => SetField(ref _selectedPair, value);
        }

        public int Dim
        {
            get => _dim;
            set
            {
                if (SetField(ref _dim, value))
                {
                    UpdateSourceStructure();
                }
            }
        }

        public int RowCount
        {
            get => _rowCount;
            set
            {
                if (SetField(ref _rowCount, value))
                {
                    UpdateSourceStructure();
                }
            }
        }

        public ObservableCollection<Pair> Source { get; } = new ObservableCollection<Pair>();

        public ICommand NavigateToRelationInputCommand => new RelayCommand(NavigateToRelationInput);
        public ICommand GoBackCommand => new RelayCommand(GoBack);
        public ICommand SaveCommand => new RelayCommand(Save);
        public ICommand AddRowCommand => new RelayCommand(AddRow);
        public ICommand RemoveRowCommand => new RelayCommand(RemoveRow, () => CanRemoveRow);
        public bool CanRemoveRow => Source.Count > 0;

        public SourceInputViewModel(IServiceProvider serviceProvider, CKL ckl) : base(serviceProvider)
        {
            _navigationService = serviceProvider.GetRequiredService<INavigationService>();
            _ckl = ckl;
            UpdateSourceStructure();
        }

        public void ReceiveParameter(CKL parameter)
        {
            _ckl = parameter;
            UpdateSourceStructure();
        }

        private void NavigateToRelationInput()
        {
            Save();
            _navigationService.NavigateTo<RelationInputViewModel, CKL>(CKL);
        }

        private void GoBack() => _navigationService.GoBack();

        private void AddRow()
        {
            RowCount = Source.Count + 1;
        }

        private void RemoveRow()
        {
            if (Source.Count > 0)
            {
                RowCount = Source.Count - 1;
            }
        }

        private void UpdateSourceStructure()
        {
            var currentData = Source.ToList();
            Source.Clear();

            for (int i = 0; i < RowCount; i++)
            {
                Pair newPair;
                if (i < currentData.Count)
                {
                    newPair = AdjustPairToDimension(currentData[i]);
                }
                else
                {
                    newPair = CreateEmptyPair();
                }
                Source.Add(newPair);
            }
        }

        private Pair CreateEmptyPair()
        {
            var values = Enumerable.Repeat(string.Empty, Dim).Cast<object>().ToList();
            return new Pair(values);
        }

        private Pair AdjustPairToDimension(Pair pair)
        {
            var newValues = new List<object>();
            int minCount = Math.Min(pair.Values.Count, Dim);

            for (int i = 0; i < minCount; i++)
            {
                newValues.Add(pair.Values[i] ?? string.Empty);
            }

            for (int i = minCount; i < Dim; i++)
            {
                newValues.Add(string.Empty);
            }

            return new Pair(newValues);
        }

        private void Save()
        {
            _ckl.Source = GenerateValidatedSource();
            OnPropertyChanged(nameof(CKL));
        }

        private HashSet<Pair> GenerateValidatedSource()
        {
            var validatedSource = new HashSet<Pair>(new Pair.PairEqualityComparer());

            foreach (var pair in Source)
            {
                if (IsValidPair(pair))
                {
                    validatedSource.Add(NormalizePair(pair));
                }
            }

            if (Dim >= 2)
            {
                var validPairs = validatedSource.ToList();
                for (int i = 0; i < validPairs.Count; i++)
                {
                    for (int j = 0; j < validPairs.Count; j++)
                    {
                        var newValues = new List<object>();

                        newValues.Add(validPairs[i].Values[0]);

                        for (int idx = 1; idx < Dim; idx++)
                        {
                            newValues.Add(validPairs[j].Values[idx]);
                        }

                        validatedSource.Add(new Pair(newValues));
                    }
                }
            }

            return validatedSource;
        }

        private bool IsValidPair(Pair pair)
        {
            for (int i = 0; i < Dim; i++)
            {
                if (i >= pair.Values.Count ||
                    string.IsNullOrEmpty(pair.Values[i]?.ToString()))
                {
                    return false;
                }
            }
            return true;
        }

        private Pair NormalizePair(Pair pair)
        {
            var normalizedValues = new List<object>();

            for (int i = 0; i < Dim; i++)
            {
                var value = i < pair.Values.Count
                    ? pair.Values[i]?.ToString().Trim() ?? string.Empty
                    : string.Empty;
                normalizedValues.Add(value);
            }

            return new Pair(normalizedValues);
        }
    }
}