   M S B A M L   `   `   `  ����F  Biw5-gallery, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:iw5_gallery.ViewModelsiw5_gallery.ViewModels  5      !    iw5_gallery.Views.AlbumView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      ;
viewModels$clr-namespace:iw5_gallery.ViewModels     5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      $
��auto��5      $
��auto��6   $	��300��6)     � DataContext  5	      �� *AlbumDetailViewModel+   ViewModelLocator �� Source# [  .��5      �  � ColumnDefinitions 5   
   �� 5      $
��Auto �6   6.   �� 5      6!    � RowDefinitions 5   
   �� 5      $
��Auto �6   6,   �� 5      6   .+�5   
   o� $��2��6   .@�5      m� $��Delete Album��6    m� Command 6-   �� *DeleteAlbumCommand+ m� CommandParameter 6T   �� *Model+m� 5      $��Delete Photo��6    6-   �� *DeletePhotoCommand+ 6T   �� *Model+5      � 5   
   $��1��6   $��0��6   $��2��6"    5      �� 5      $
��Auto �6#   62   �� 5      $��* �6#   6/    5      �� 5      $
��Auto �6    60   �� 5       $
��Auto �6    60   .+�5"      �� $��
Album Name��6   6+   �� 5#      $��1��6   ��6&   �� *
Model.Name+6D   �� 5$      $��1��6   $��0��6!   $��Photos��61   6D   �� 5%      -   5         ��Name$ AlbumPhotosListBox��5%   3   $��1��6   $��1��6#   $	��456��6y   $	��110��6�   ��6O   ��  �� Path$ Model.Photos�.  (System.Windows.Interactivity.Interaction   Triggers 5&      /  )System.Windows.Interactivity.EventTrigger  5'      	   	EventName$	 SelectionChanged��6%   .  (System.Windows.Interactivity.TriggerBase
   Actions.
 5(      6  0System.Windows.Interactivity.InvokeCommandAction     Command 60   �� *SelectAlbumPhotoCommand+   CommandParameter 5)   0   ��  �� ElementName$ AlbumPhotosListBox��$ SelectedItem�5*      ��5,      �� 5-      .R�5.      ��  �� Orientation$ Vertical=�6%   .+�5/      �� ��6(   �� $
 Name�6E   �� 50      $
��Left=�6v   $��55��6�   $��70��6�   $	��Top=�6�   ��6$   �� $ Image�  ImageToSourceConverter �� 	Converter# [ 6�   51      52      54      55      56      57      