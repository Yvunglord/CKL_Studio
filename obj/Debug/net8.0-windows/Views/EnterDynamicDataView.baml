   M S B A M L   `   `   `  ����E  ACKL_Studio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:CKL_Studio.EnterDynamicDataCKL_Studio.EnterDynamicData  5      =#clr-namespace:CKL_Studio.ViewModelsCKL_Studio.ViewModels  5      6    0CKL_Studio.EnterDynamicData.EnterDynamicDataView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ;local)clr-namespace:CKL_Studio.EnterDynamicData     5      2vm#clr-namespace:CKL_Studio.ViewModels     5      .��5
      � ���#1e1e1e  � RowDefinitions  5   
   �� 5      $
��Auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��2* �6   �� 5      $��* �6   .+�5   
   ��  
  Header#��[  6   $%��Настроить проект��60   $��60, 40, 0, 60q�6K   $
��Left=�6b   $	��Top=�6}   �� 5   
     h1#��[ 6   $7��1Начните вводить множество:��6,   $��60, 0, 0, 0q�6Q   ]� 5   
   $��1��6   $��1��6!   $��Stretch=�61   $��Stretch=�6O   .+�5      �� -   5         ��Name$ 	btnImport��5   f   $	��Top=�6     
btTemplate#��[ 6*   $	��Top=�6N   $��Импорт��6y   $��0, 10, 0, 0q�6�   �� 5      $��Bottom=�6    �� Orientation$ 
Horizontal=�61   $��Bottom=�6J   $��Right=�6e   .+�5      �  5      �� 5      $
��Auto �6+   �� 5      $
��Auto �6+     5       �� 5!      $
��Auto �6(   �� 5"      $
��Auto �6(   .+�5%      �� -   5        $��btnAdd��5%   ^   $��0��6   $��1��6*     
btNavigate#��[ 6:   $��Добавить��6l   $��0, 0, 20, 10q�6   ��6�   �� *31DataContext.NavigateToEnterDynamicDataViewCommand+ �� RelativeSource ��  �� AncestorType
 Window6  �� 5&      -   5        $��
btnForward��5&   ^   $��1��6   $��1��6*   #��[ 6:   $��
Далее��6p   $��0, 0, 20, 20q�6�   ��6�   �� */-DataContext.NavigateToMakeRelationViewCommand+ �� 
 Window�� 5'      -   5        $��btnBack��5'   b   $��2��6   $��0��6.   #��[ 6>   $��
Назад��6q   $��0, 0, 10, 20q�6�   ��6�   �� *20DataContext.NavigateToEnterStaticDataViewCommand+ �� 
 Window5(      5)      5*      � 5,   
   $��1��6   $��0��6   $��	60,0,73,0q�6,     5-      �� 5.      $
��Auto �6    �� 5/      $��* �6    .+�52      �� $��1��6   $ 
Horizontal=�6&   $��Stretch=�6?   $��Stretch=�6]   .+�53      �� $	��350��6   $	��350��6*   $��
10,10,0,10q�69   $��Stretch=�6M   $��Stretch=�6k   ���#1e1e1e$��0q�6�     CustomListBoxItemStyle#��[ 54      ��55      �� *Items+��56      �� 57      .R�58      $�  K� IsItemsHost$
 True��65   59      ��5;      �� 5<      .R�5=      �� ���#373737$��Stretch=�6:   ._�5>   "   � $��50��6'   $��Stretch=�63    5?   &   �� 5@   *   $��* �6;   �� 5A   *   $
��Auto �6;     5C   &   �� 5D   *   $
��Auto �68   �� 5E   *   $��* �68   5G   #   5H      5I      5K      �� 5L      $��20��6   $��20��6#   $	��Top=�6/   $��10,10,10,10q�6G   ���#2c2c2c��6q   �� *AddItemCommand+��5M      F� 5N      +����    @KL р�� @KL �@KL     �@KL ��� 	
�������$��2��6K   $��Center=�5O      $��Center=�66    �� Stretch$ Uniform=�5P      5R      5T      5U      5V      5W      