   M S B A M L   `   `   `  ����E  AViewModels, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:ViewModels;assembly=ViewModels
ViewModels  5      @ <Views, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Views.SelectionsViews.Selections 5      %   Views.Selections.SelectionsView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      C
viewModels,clr-namespace:ViewModels;assembly=ViewModels     5      5
selectionsclr-namespace:Views.Selections     5      
�������
���   �$��1q�5      $��1q�5        � 	Resources  5       �� 
TargetType "  TextBlock.RightAlignedTagStyle �� BasedOn��%0   '��       7   �� 5   
   " ��6   8   60   5      .��5      ��  �� Orientation$ Vertical=�6   .+�5   
   �� $��
Selections��6      TextBlock.BigBoldHeaderStyle#��[ 6&   �� 5   
   -   5        $��Disabled=�5      ��5      �� *-+CurrentSelectionViewModel.AllTyrePlacements+��5      �� 5       �� DataType'   !ViewModels.TyrePlacementViewModel"  6   .R�5      1  +Views.Selections.TyrePlacementSelectionView  ��6<   ��   Selections.ColumnWidth �� Source# [ 5      ��5      �� 5       .R�5!      A�  A� Rows$ 2��6"    A� Columns$ 2��6+   5"      5$      �� 5'   
   $ 
Horizontal=�6   $��50��6.   .+�5(      �� $ 
Horizontal=�6   ��62   �� # [ .+�5)      �� $��Track��6   
���   �6)   �� 5*      -   5        $��TracksComboBox��5+       �� DisplayMemberPath$
 Name��5.      $��Center=�50      $	��0 3q�51      ��5,      �� *TracksDetailsCollection+��5/      ��   Selections.ComboBox.Width# [ 51      52      �� 53      $ 
Horizontal=�6   ��62   �� # [ .+�54      �� $��Temperature��6   �� 55      $��80��56      $	��0 3q�57      	 �� MaxLines$	 1��59      "
 �� VerticalScrollBarVisibility$
 Disabled=�5:      $ �� HorizontalScrollBarVisibility$ Disabled=�5;       �� TextAlignment$ Center=�5<      ��58      �� *42CurrentSelectionViewModel.SelectedTrackTemperature+ �� UpdateSourceTrigger$ PropertyChanged=�5<      5=      5>      5?      5@      