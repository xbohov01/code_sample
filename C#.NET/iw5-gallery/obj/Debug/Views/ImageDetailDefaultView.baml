   M S B A M L   `   `   `  ����F  Biw5-gallery, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:iw5_gallery.ViewModelsiw5_gallery.ViewModels  5   	   .    (iw5_gallery.Views.ImageDetailDefaultView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ;
viewModels$clr-namespace:iw5_gallery.ViewModels     5   	   $	��600��5
   	   $
��Auto��6   $	��Top=�6#     � DataContext  5   	   �� *ImageDetailViewModel+   ViewModelLocator �� Source# [  .��5      � $��1��6   $��0��6    � ColumnDefinitions 5   
   �� 5      $	��21* �6   �� 5      $
��170* �6   .+�5   
   � $��2��6   $��0,0,0,0q�6#    � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $	��370 �6     5      �� 5      $
��Auto �6#   �� 5      .+�5      �� $��1��6   $��0��6!   $��Name: ��62   6E   �� 5       $��2��6   $��0��6!   $��Date and time: ��62   6N   �� 5!      $��4��6   $��0��6!   $��Format: ��62   6G   �� 5"      $��5��6   $��0��6!   $��Resolution: ��62   6K   �� 5#      $��3��6   $��0��6!   $��Note: ��62   6E   �� 5$      $��1��6   $��1��6!   ��61   ��  �� Path$ Detail.Name��� 5%      $��2��6   $��1��6!   ��61   �� $ Detail.Date��� 5&      $��4��6   $��1��6!   ��61   �� $ ViewImage.RawFormat��� 5'      $��5��6   $��1��6!   ��61   �� *ViewImage.Size+�� 5(      $��3��6   $��1��6!   ��61   �� $ Detail.Description�(� 5)       (� Stretch$
 Fill=�6   $��6��6%   $��0��62   $��2��6B   .��5*      �� ��6   �� $ 	ViewImage�  ImageToSourceConverter �� 	Converter# [ 5+      5,      5-      5.      