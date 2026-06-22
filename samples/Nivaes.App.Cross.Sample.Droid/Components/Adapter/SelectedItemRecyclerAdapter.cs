using System;
using System.Diagnostics.CodeAnalysis;
using Android.Runtime;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Nivaes.App.Cross.Droid;
using Nivaes.App.Cross.Droid.RecyclerView;
using Nivaes.App.Cross.Sample.Droid;
using Resource = Nivaes.App.Cross.Sample.Droid.Resource;

namespace Playground.Droid.Adapter
{
    public partial class SelectedItemRecyclerAdapter : MvxRecyclerAdapter
    {
        public event EventHandler<SelectedItemEventArgs>? OnItemClick;

        public SelectedItemRecyclerAdapter(IMvxAndroidBindingContext? bindingContext)
              : base(bindingContext)
        {
        }

        [DynamicDependency(nameof(SelectedItemRecyclerAdapter), typeof(SelectedItemRecyclerAdapter))]
        protected SelectedItemRecyclerAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var itemLogo = holder.ItemView.FindViewById<ImageView>(Resource.Id.img_logo);
            AndroidX.Core.View.ViewCompat.SetTransitionName(itemLogo, "anim_img" + position);

            base.OnBindViewHolder(holder, position);
        }

        protected override void OnItemViewClick(object? sender, EventArgs e)
        {
            base.OnItemViewClick(sender, e);

            var holder = (MvxRecyclerViewHolder?)sender;
            OnItemClick?.Invoke(this, new SelectedItemEventArgs(holder!.AdapterPosition, holder?.ItemView, holder?.DataContext));
        }
    }
}
