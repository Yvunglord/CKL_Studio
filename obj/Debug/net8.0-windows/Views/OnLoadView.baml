   M S B A M L   `   `   `  ����E  ACKL_Studio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:CKL_Studio.OnLoadCKL_Studio.OnLoad  5      "    CKL_Studio.OnLoad.OnLoadView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:CKL_Studio.OnLoad     5      .��5	      � ���#1e1e1e  � RowDefinitions  5
   
   �� 5      $
��Auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��2* �6   �� 5      $��* �6   .+�5   
   ��  
  Header#��[  6   $��
CKL Studio��60   $��60, 40, 0, 60q�6E   $
��Left=�6\   $	��Top=�6w   �� 5   
     h1#��[ 6   $(��"Открыть последние:��6,   $��60, 0, 0, 0q�6I   �� 5   
   $��1��6   $��1��6"    �� Orientation$ Vertical=�62   $
��Left=�6I   $	��Top=�6d   $��	0,43,10,0q�6|   .+�5      �� -   5        $��	btnCreate��5   9     
btTemplate#��[ 6   $��Создать��6J   ��6\   �� *20DataContext.NavigateToEnterStaticDataViewCommand+ �� RelativeSource ��  �� AncestorType
 Window�� 5      -   5        $��btnOpen��5   9   #��[ 6   $��Открыть��6H   6Z   �� 5      -   5        $��btnOpenDirectory��5   9   #��[ 6   $2��,Открыть локальную папку��6Q   5      � 5   
   $��1��6   $��0��6   $��	60,0,73,0q�6,     5      �� 5       $
��Auto �6    �� 5!      $��* �6    .+�5$      �� -   5          CustomComboBoxStyle#��[ 5$      $	��350��6D   $��	10,10,0,0q�6P   $
��Left=�6c   $��0��6~    �� 
IsEditable$
 True��5'       �� IsTextSearchEnabled$
 True��5(        ComboBoxItemStyle#��[ 5)      ��5%      �� *SearchHistory+ �� Text 5&      �� *
SearchText+ �� UpdateSourceTrigger$ PropertyChanged=�5*      �� 5,      $��1��6   $	��350��6#   $��
10,10,0,10q�63   $��Stretch=�6G   $��Stretch=�6e   ���	#FF2C2C2C$��0q�6�   ��5-      �� *FilteredFiles+��5.      �� 5/      .R�50      $� 	 K� IsItemsHost$
	 True��61   51      ��53      �� 54      
 �� 
TargetType"
 ��6   .�55      ��  �� Property � �� �� Value$ 50��63   �� 56       � �� �	#FF2C2C2C68   �� 57       � �� �	#FF2C2C2C67   �� 58       � ��$ 0q�64   �� 59       � ��$ Stretch=�6G   �� 5:       � ��$ 0,0,0,1q�63   5;      ��5=      �� 5>      .R�5?      �� ���#373737$��Stretch=�66   ._�5@      � $��50��6#   $��Stretch=�6/    5A   "   �� 5B   &   $��* �67   �� 5C   &   $
��Auto �67     5E   "   �� 5F   &   $
��Auto �64   �� 5G   &   $��* �64   .+�5J   "   �� $��14(�6F   
�������$��Medium"�6g   $��5,0,0,0q�6{   $	��Top=�6�   ��6,   �� *
FileName+6�   �� 5K   "   $��12(�6F   
�������$��Regular"�6g   $��5,5,0,0q�6|   $	��Top=�6�   $��1��6�   ��6,   �� *
FilePath+6�   �� 5L   "   $��12(�6i   
�������$��Regular"�6�   $��5,0,5,0q�6�   $��Center=�6�   $��Right=�6�   $��1��6�   $��2��6�   ��6,   �� *
LastChange+ �� StringFormat$ dd.MM.yyyy HH:mm��6�   5M      5N      5O      5Q      5R      5S      5T      