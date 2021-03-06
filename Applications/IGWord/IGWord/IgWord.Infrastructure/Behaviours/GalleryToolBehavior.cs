﻿using Infragistics.Windows;
using Infragistics.Windows.Ribbon;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media.Imaging;

namespace IgWord.Infrastructure.Behaviours
{
    public class GalleryToolBehavior : Behavior<GalleryTool>
    {
        #region GroupKeysProperty

        public static readonly DependencyProperty GroupKeysProperty = DependencyProperty.RegisterAttached("GroupKeys", typeof(IEnumerable), typeof(GalleryToolBehavior), new PropertyMetadata(null, OnGroupKeysChanged));

        public static void SetGroupKeys(GalleryItemGroup source, object value)
        {
            source.SetValue(GroupKeysProperty, value);
        }

        public static object GetGroupKeys(GalleryItemGroup source)
        {
            return (object)source.GetValue(GroupKeysProperty);
        }

        private static void OnGroupKeysChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GalleryItemGroup galleryItemGroup = d as GalleryItemGroup;

            if (e.NewValue == null)
                return;

            galleryItemGroup.ItemKeys.Clear();

            foreach (var item in (IEnumerable)e.NewValue)
            {
                galleryItemGroup.ItemKeys.Add(item.ToString());
            }
        }

        #endregion //GroupKeysProperty

        #region ItemsSourceProperty

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(GalleryToolBehavior), new PropertyMetadata(null, new PropertyChangedCallback(OnItemsSourceChanged)));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GalleryToolBehavior behavior = d as GalleryToolBehavior;
            if (behavior != null)
                behavior.OnItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
        }

        private void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            if (newValue != null && AssociatedObject != null)
            {
                foreach (var item in newValue)
                {
                    AssociatedObject.Items.Add(CreateBulletGalleryItem(item.ToString()));
                }
            }
        }

        #endregion //ItemsSourceProperty

        #region ImagesPathProperty

        public static readonly DependencyProperty ImagesPathProperty = DependencyProperty.Register("ImagesPath", typeof(string), typeof(GalleryToolBehavior), new PropertyMetadata(null));

        public string ImagesPath
        {
            get { return (string)GetValue(ImagesPathProperty); }
            set { SetValue(ImagesPathProperty, value); }
        }
        #endregion //ImagesPathProperty

        #region SelectedItemKeyProperty

        public static readonly DependencyProperty SelectedItemKeyProperty = DependencyProperty.Register("SelectedItemKey", typeof(string), typeof(GalleryToolBehavior), new PropertyMetadata(null, new PropertyChangedCallback(OnSelectedItemKeyChanged)));

        public string SelectedItemKey
        {
            get { return (string)GetValue(SelectedItemKeyProperty); }
            set { SetValue(SelectedItemKeyProperty, value); }
        }

        private static void OnSelectedItemKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GalleryToolBehavior behavior = d as GalleryToolBehavior;
            if (behavior != null)
                behavior.OnSelectedItemKeyChanged((string)e.OldValue, (string)e.NewValue);
        }

        private void OnSelectedItemKeyChanged(string oldValue, string newValue)
        {
            if (AssociatedObject.Items.ContainsKey(newValue))
            {
                AssociatedObject.SelectedItem = AssociatedObject.Items[newValue];
            }
        }

        #endregion //SelectedItemKeyProperty

        #region OnAttached

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.ItemSelected += AssociatedObject_ItemSelected;
        }

        #endregion //OnAttached

        #region OnDetaching

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.ItemSelected -= AssociatedObject_ItemSelected;
        }

        #endregion //OnDetaching

        #region AssociatedObject_ItemSelected

        private void AssociatedObject_ItemSelected(object sender, Infragistics.Windows.Ribbon.Events.GalleryItemEventArgs e)
        {
            SelectedItemKey = e.Item.Key;
        }

        #endregion //AssociatedObject_ItemSelected

        #region CreateBulletGalleryItem

        private GalleryItem CreateBulletGalleryItem(string bulletStyle)
        {
            GalleryItem galleryItem = new GalleryItem();
            galleryItem.Key = bulletStyle;
            galleryItem.Text = bulletStyle;
            galleryItem.Image = new BitmapImage(new Uri(String.Format("{0}{1}.png", ImagesPath, bulletStyle), UriKind.RelativeOrAbsolute));
            return galleryItem;
        }

        #endregion //CreateBulletGalleryItem
    }
}
