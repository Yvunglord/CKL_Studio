   M S B A M L   `   `   `  ����E  ACKL_Studio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:CKL_Studio.EnterStaticDataCKL_Studio.EnterStaticData  5      4    .CKL_Studio.EnterStaticData.EnterStaticDataView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :local(clr-namespace:CKL_Studio.EnterStaticData     5      .��5	      � ���#1e1e1e  � ColumnDefinitions  5
   
   �� 5      $��2* �6   �� 5      $��* �6   .+�5   
   �� $��1��6    �� Orientation$ 
Horizontal=�6%   $��Bottom=�6>   $��Right=�6Y   $��
0, 0, 0, 0q�6u   .+�5      �� -   5        $��btnBack��5   9      
btNavigate#��[  6   $��
Назад��6H   $��0, 0, 10, 20q�6X   ��6n   �� *)'DataContext.NavigateToOnLoadViewCommand+ �� RelativeSource ��  �� AncestorType
 Window�� 5      -   5        $��
btnForward��5   9   #��[  6   $��
Далее��6K   $��0, 0, 20, 20q�6[   ��6q   �� *31DataContext.NavigateToEnterDynamicDataViewCommand+ �� 
 Window5      �� 5   
   $��0��6   $ Vertical=�6%   $	��Top=�6<   $
��Left=�6T   $��20, 20, 0, 0q�6o   .+�5      ��  
 Header#��[ 6   $0��*Настроить новый проект��64   �� 5        h1#��[ 6   $N��HФайл алгебры динамических отношений .ckl��60   �� 5        h2#��[ 6   $��Имя файла��60   �� 5        CustomTextBoxStyle#��[ 6   $	��390��6B   $��
6, 0, 0, 0q�6N   $
��Left=�6�   ��6b   �� *Name+ �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5      #��[ 6   $��Путь к файлу��60   $��0, 10, 0, 0q�6G   �� 5      #��[ 6   $	��390��6B   $��
6, 0, 0, 0q�6N   $
��Left=�6�   ��6b   �� *
FilePath+$ PropertyChanged=�6�   �� 5      #��[ 6   $>��8Глобальный временной интервал��60   $��0, 10, 0, 0q�6X   �� 5      $ 
Horizontal=�6   .+�5      �� -   5        $��timeComboBox��5        CustomComboBoxStyle#��[ 5      $	��390��5      $��
6, 0, 0, 0q�5       �� 
IsEditable$
 True��5       �� 
IsReadOnly$
 True��5        �� SelectedItem 5!      �� *SelectedTimeUnit+ �� Mode$ TwoWay=�$ PropertyChanged=�.@�5"      �� $��атс��6#   �� 5#      $��фмс��6#   �� 5$      $��пкс��6#   �� 5%      $
��нс��6#   �� 5&      $��мкс��6#   �� 5'      $
��мс��6#   �� 5(      $��сек��6#   �� 5)      $��мин��6#   �� 5*      $��ч��6#   �� 5+      $��нед��6#   5,      �� 5-      #��[ 6   ��5.      �� *SelectedTimeUnit.Content+5/      50      �� 51   
   $��1��6   52      53      