   M S B A M L   `   `   `  ����F  Biw5-gallery, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:iw5_gallery.ViewModelsiw5_gallery.ViewModels  5   	   )    #iw5_gallery.Views.ImageCreateWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ;
viewModels$clr-namespace:iw5_gallery.ViewModels     5   	     � Title$  	Add photo��5
   	   $	��500��6   $	��500��6(    � DataContext 5   	   �� *ImageDetailViewModel+   ViewModelLocator �� Source# [  .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   o� .@�5      m� $
��Save��6    m� Command 6%   �� *SaveImageCommand+ m� CommandParameter 6J   �� *Detail+5      � 5   
   $��1��6    � ColumnDefinitions 5      �� 5      $	��21* �6#   �� 5      $
��170* �6#   .+�5      � $��2��6   $��0,0,0,0q�6'    5      �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5       $
��Auto �6$   �� 5!      $
��Auto �6$    5#      �� 5$      $
��Auto �6'   �� 5%      .+�5'      �� $��1��6   $��0��6%   $��Name: ��66   6I   �� 5(      $��2��6   $��0��6%   $��Date and time: ��66   6R   �� 5)      $��4��6   $��0��6%   $��Format: ��66   6K   �� 5*      $��5��6   $��0��6%   $��Resolution: ��66   6O   �� 5+      $��3��6   $��0��6%   $��Note: ��66   6I   �� 5,      $��1��6   $��1��6'   ��67   ��  �� Path$
 Name� �� UpdateSourceTrigger$ PropertyChanged=�(  "System.Windows.Controls.DatePicker  5-      $��2��6   $��1��6*   	   SelectedDate	 6:   �� *Detail.Date+�� 5.      $��4��6   $��1��6%   ��65   �� *ViewImage.RawFormat+�� 5/      $��5��6   $��1��6%   ��65   �� *ViewImage.Size+�� 50      $��3��6   $��1��6'   ��67   �� $ Detail.Description�$ PropertyChanged=��� 51      $��6��6   $��0��6&   $��	Open file��6[   ��66   �� *FindImageCommand+6q   52      53      � 54   
   $��2��6   .+�55      (� .��56      �� ��6   �� $ 	ViewImage�  ImageToSourceConverter
 �� 	Converter#
 [ 57      58      59      5:      