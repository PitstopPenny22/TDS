   M S B A M L   `   `   `  ����L  HWpfCustomControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullP:clr-namespace:WpfCustomControls;assembly=WpfCustomControlsWpfCustomControls  5      E AViewModels, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:ViewModels;assembly=ViewModels
ViewModels 5      	 Views"   Views.Results.ValueStateView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      XwpfCustomControls:clr-namespace:WpfCustomControls;assembly=WpfCustomControls     5      C
viewModels,clr-namespace:ViewModels;assembly=ViewModels     5      -   5        $��ValueStateControl��5	      $��1q�5      
���   �$��Center=�5      $��Center=�5      .��5      3   -WpfCustomControls.BorderedBackgroundTextBlock  $��Center=�5   	   $��Center=�5   	       	TextValue  5   	   �� *Value+��5   	   ��  �� ElementName$ ValueStateControl�� �� Path$ ActualHeight���5   	   �� $ ValueStateControl��$ ActualWidth���5   
   �� 5       �� 
TargetType"  6   .�5      ��  �� Property � �� �� Value 6/   �� $ ValueStateControl��$ 
Background� �� Triggers 5      �� 5      5  /ViewModels.ValueStateViewModel+DegradationState   Green �� Value# Z 6<   	 �� Binding	 6"   �� *State+.a�5      ��  � ��
 ����67   5      �� 5      
   Yellow# Z
 6<   	 6"   �� *State+.a�5       ��  � ��
 � ���67   5!      �� 5"      
   Red# Z 6<   	 6"   �� *State+.a�5#      ��  � ��
 �  ��67   5$      �� 5%         NotValid# Z 6<   	 6"   �� *State+.a�5&      ��  � ��
 �����67   5'      5)      5+      5,      