   M S B A M L   `   `   `  ����F  Biw5-gallery, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:iw5_gallery.ViewModelsiw5_gallery.ViewModels  5   	   !    iw5_gallery.Views.ImageView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ;
viewModels$clr-namespace:iw5_gallery.ViewModels     5   	   c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5   	   $	��300��5   	   $	��350��6     � DataContext  5	   	   �� *ImageDetailViewModel+   ViewModelLocator �� Source# [  .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   6+   �� 5      $	��100 �6   6-   �� 5      $	��100 �6   6-   .+�5   
   o� .@�5      m� $��Update��6    m� Command 6'   �� *DeleteImageCommand+ m� CommandParameter 6N   �� *Detail+m� 5      $��Delete��6    6'   �� *DeleteImageCommand+ 6N   �� *Detail+5      � 5   
   $��1��6   $��0��6    5      �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5       $
��Auto �6$   �� 5!      $
��Auto �6$   �� 5"      $
��Auto �6$   �� 5#      $
��Auto �6$   �� 5$      $	��345 �6$    5&      �� 5'      $
��Auto �6'   �� 5(      .+�5*      �� $��1��6   $��0��6%   $��Name: ��66   6I   �� 5+      $��2��6   $��0��6%   $��Date and time: ��66   6R   �� 5,      $��4��6   $��0��6%   $��Format: ��66   6K   �� 5-      $��5��6   $��0��6%   $��Resolution: ��66   6O   �� 5.      $��3��6   $��0��6%   $��Note: ��66   6I   �� 5/      $��1��6   $��1��6%   ��65   ��  �� Path$ Detail.Name��� 50      $��2��6   $��1��6%   ��65   �� $ Detail.Date��� 51      $��4��6   $��1��6%   ��65   �� $ ViewImage.RawFormat��� 52      $��5��6   $��1��6%   ��65   �� *ViewImage.Size+�� 53      $��3��6   $��1��6%   ��65   �� $ Detail.Description�(� 54       (� Stretch$
 Fill=�6   $��6��6%   $��0��62   $��2��6B   .��55      �� ��6   �� $ 	ViewImage�  ImageToSourceConverter �� 	Converter# [ 56      57      �� 58   
   $��Persons��6   $��1��6"   $��0��62   6A   � 59   
   $��1��6   $��1��6   $	��Top=�6,    5:      �� 5;      $
��Auto �6    60   �� 5<      $��* �6    6-   .+�5>      o� .@�5?      m� $��Untag��6    6*   �� *UntagPersonCommand+5@      �� 5A   
   -   5        	 ��Name$	 PersonListBox��5A      $��1��6   $��457.5��6a   ��66   �� $ Detail.People�.  (System.Windows.Interactivity.Interaction
   Triggers
 5B      /  )System.Windows.Interactivity.EventTrigger  5C         	EventName$ SelectionChanged��6!   .  (System.Windows.Interactivity.TriggerBase   Actions. 5D      6  0System.Windows.Interactivity.InvokeCommandAction     Command 6,   �� *SelectPersonCommand+   CommandParameter 5E   ,   ��  �� ElementName$ PersonListBox��$ SelectedItem�5F      ��5H      �� 5I      .R�5J      ��  �� Orientation$ Vertical=�6!   .+�5K      �� ��6$   �� $
 Name�6A   5L      5M      5O      5P      �� 5Q   
   $��Objects��6   $��2��6"   $��0��62   6A   � 5R   
   $��2��6   $��1��6   $	��top=�6,    5S      �� 5T      $
��Auto �6    60   �� 5U      $��* �6    6-   .+�5W      o� .@�5X      m� $��Untag��6    6*   �� *UntagObjectCommand+5Y      �� 5Z   
   -   5        $	 ObjectListBox��5Z      $��1��6   $��457.5��6b   ��66   �� $ Detail.Objects�
 5[        5\      $ SelectionChanged��6!   . 5]         6,   �� *SelectObjectCommand+ 5^   ,   �� $ ObjectListBox��$ SelectedItem�5_      ��5a      �� 5b      .R�5c      �� $ Vertical=�6!   .+�5d      �� ��6$   �� $
 Name�6A   5e      5f      5h      5i      5j      5k      