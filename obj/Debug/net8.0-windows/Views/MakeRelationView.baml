   M S B A M L   `   `   `  ����E  ACKL_Studio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:CKL_Studio.ViewsCKL_Studio.Views  5      '    !CKL_Studio.Views.MakeRelationView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:CKL_Studio.Views     5      .��5	      � ���#1e1e1e  � RowDefinitions  5
   
   �� 5      $
��Auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��2* �6   �� 5      $��* �6   .+�5   
   ��  
  Header#��[  6   $%��Настроить проект��60   $��60, 40, 0, 60q�6K   $
��Left=�6b   $	��Top=�6}   �� 5   
     h1#��[ 6   $,��&Составьте отношение:��6,   $��60, 0, 0, 0q�6K   �� 5   
   $��1��6   $��1��6%    �� Orientation$ 
Horizontal=�62   $��Bottom=�6K   $��Right=�6f   $��
0, 0, 0, 0q�6�   .+�5      �� -   5        $��btnBack��5   9     
btNavigate#��[ 6   $��
Назад��6H   $��0, 0, 10, 20q�6X   ��6o   �� *31DataContext.NavigateToEnterDynamicDataViewCommand+ �� RelativeSource ��  �� AncestorType
 Window�� 5      -   5        $��
btnForward��5   9   #��[ 6   $��
Далее��6K   $��0, 0, 20, 20q�6[   ��6q   �� *'%DataContext.NavigateToMainViewCommand+ �� 
 Window5      � 5   
   $��1��6   $��0��6   $��	60,0,73,0q�6,     5      �� 5      $
��Auto �6    �� 5      $��* �6    .+�5"      �� $��1��6   $ 
Horizontal=�6&   $��Stretch=�6?   $��Stretch=�6]   .+�5#      �� $	��350��6   $	��350��6*   $��
10,10,0,10q�69   $��Stretch=�6M   $��Stretch=�6k   ���#1e1e1e$��0q�6�     CustomListBoxItemStyle#��[ 5$      ��5%      �� *Items+��5&      �� 5'      .R�5(      $�  K� IsItemsHost$
 True��65   5)      ��5+      �� 5,      .R�5-      �� ���#373737$��Stretch=�6:   ._�5.   "   � $��50��6'   $��Stretch=�63    5/   &   �� 50   *   $��* �6;   �� 51   *   $
��Auto �6;     53   &   �� 54   *   $
��Auto �68   �� 55   *   $��* �68   57   #   58      59      5;      �� 5<      $��20��6   $��20��6$   $	��Top=�60   $��10,10,10,10q�6H   ���#2c2c2c��6r   �� *AddItemCommand+��5=      F� 5>      +����    @KL р�� @KL �@KL     �@KL ��� 	
�������$��2��6K   $��Center=�5?      $��Center=�62    �� Stretch$ Uniform=�5@      5B      5D      5E      5F      5G      