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
            return Dim switch
            {
                1 => new Pair(string.Empty),
                2 => new Pair(string.Empty, string.Empty),
                3 => new Pair(string.Empty, string.Empty, string.Empty),
                _ => new Pair(string.Empty)
            };
        }

        private Pair AdjustPairToDimension(Pair pair)
        {
            return Dim switch
            {
                1 when pair.SecondValue != null || pair.ThirdValue != null
                    => new Pair(pair.FirstValue ?? string.Empty),

                2 when pair.ThirdValue != null
                    => new Pair(pair.FirstValue ?? string.Empty, pair.SecondValue ?? string.Empty),

                2 when pair.SecondValue == null
                    => new Pair(pair.FirstValue ?? string.Empty, string.Empty),

                3 when pair.ThirdValue == null
                    => new Pair(
                        pair.FirstValue ?? string.Empty,
                        pair.SecondValue ?? string.Empty,
                        string.Empty),

                _ => pair
            };
        }

        private void Save()
        {
            _ckl.Source = GenerateValidatedSource();
            OnPropertyChanged(nameof(CKL));
        }

        private HashSet<Pair> GenerateValidatedSource()
        {
            var validatedSource = new HashSet<Pair>(new PairComparer());

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
                        if (Dim == 2)
                        {
                            validatedSource.Add(new Pair(
                                validPairs[i].FirstValue,
                                validPairs[j].SecondValue ?? string.Empty));
                        }
                        else if (Dim == 3)
                        {
                            validatedSource.Add(new Pair(
                                validPairs[i].FirstValue,
                                validPairs[j].SecondValue ?? string.Empty,
                                validPairs[j].ThirdValue ?? string.Empty));
                        }
                    }
                }
            }

            return validatedSource;
        }

        private bool IsValidPair(Pair pair)
        {
            if (string.IsNullOrEmpty(pair.FirstValue?.ToString()))
                return false;

            if (Dim >= 2 && string.IsNullOrEmpty(pair.SecondValue?.ToString()))
                return false;

            if (Dim >= 3 && string.IsNullOrEmpty(pair.ThirdValue?.ToString()))
                return false;

            return true;
        }

        private Pair NormalizePair(Pair pair)
        {
            return Dim switch
            {
                1 => new Pair(pair.FirstValue.ToString().Trim()),
                2 => new Pair(
                    pair.FirstValue.ToString().Trim(),
                    pair.SecondValue.ToString().Trim()),
                3 => new Pair(
                    pair.FirstValue.ToString().Trim(),
                    pair.SecondValue.ToString().Trim(),
                    pair.ThirdValue.ToString().Trim()),
                _ => pair
            };
        }
    }

    public class PairComparer : IEqualityComparer<Pair>
    {
        public bool Equals(Pair x, Pair y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;

            return string.Equals(x.ToString(), y.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(Pair obj)
        {
            return obj.ToString().ToLower().GetHashCode();
        }
    }
}