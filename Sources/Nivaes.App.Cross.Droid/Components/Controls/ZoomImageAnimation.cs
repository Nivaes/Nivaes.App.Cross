using Android.Animation;
using Android.Graphics;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace Nivaes.App.Cross.Droid
{
    // ToDo: Mover a libreria
    public class ZoomImageAnimation
    {
        private Animator currentAnimator;
        private const int ShortAnimationDuration = 200;

        private readonly View mThumbView;
        private readonly View mFrameContainer;
        private readonly ImageView mExpandedImageView;

        public ZoomImageAnimation(View thumbView, View frameContainer, ImageView expandedImageView)
        {
            mThumbView = thumbView;
            mFrameContainer = frameContainer;
            mExpandedImageView = expandedImageView;
        }

        public void ZoomImageFromThumb(Android.Net.Uri imageUri)
        {
            currentAnimator?.Cancel();

            // Load the high-resolution "zoomed-in" image.
            mExpandedImageView.SetImageURI(imageUri);

            // Calculate the starting and ending bounds for the zoomed-in image.
            // This step involves lots of math. Yay, math.
            var startBounds = new Rect();
            var finalBounds = new Rect();
            var globalOffset = new Point();

            // The start bounds are the global visible rectangle of the thumbnail,
            // and the final bounds are the global visible rectangle of the container
            // view. Also set the container view's offset as the origin for the
            // bounds, since that's the origin for the positioning animation
            // properties (X, Y).
            mThumbView.GetGlobalVisibleRect(startBounds);
            mFrameContainer.GetGlobalVisibleRect(finalBounds, globalOffset);
            mFrameContainer.SetBackgroundColor(Color.Black);

            startBounds.Offset(-globalOffset.X, -globalOffset.Y);
            finalBounds.Offset(-globalOffset.X, -globalOffset.Y);

            // Adjust the start bounds to be the same aspect ratio as the final
            // bounds using the "center crop" technique. This prevents undesirable
            // stretching during the animation. Also calculate the start scaling
            // factor (the end scaling factor is always 1.0).
            float startScale;
            if ((float)finalBounds.Width() / finalBounds.Height()
                    > (float)startBounds.Width() / startBounds.Height())
            {
                // Extend start bounds horizontally
                startScale = (float)startBounds.Height() / finalBounds.Height();
                float startWidth = startScale * finalBounds.Width();
                float deltaWidth = (startWidth - startBounds.Width()) / 2;
                startBounds.Left -= (int)deltaWidth;
                startBounds.Right += (int)deltaWidth;
            }
            else
            {
                // Extend start bounds vertically
                startScale = (float)startBounds.Width() / finalBounds.Width();
                float startHeight = startScale * finalBounds.Height();
                float deltaHeight = (startHeight - startBounds.Height()) / 2;
                startBounds.Top -= (int)deltaHeight;
                startBounds.Bottom += (int)deltaHeight;
            }

            // Hide the thumbnail and show the zoomed-in view. When the animation
            // begins, it will position the zoomed-in view in the place of the
            // thumbnail.
            mThumbView.Alpha = 0.0f;
            mExpandedImageView.Visibility = ViewStates.Visible;

            // Set the pivot point for SCALE_X and SCALE_Y transformations
            // to the top-left corner of the zoomed-in view (the default
            // is the center of the view).
            mExpandedImageView.PivotX = 0.0f;
            mExpandedImageView.PivotY = 0.0f;

            // Construct and run the parallel animation of the four translation and
            // scale properties (X, Y, SCALE_X, and SCALE_Y).
            AnimatorSet set = new AnimatorSet();
            set.Play(ObjectAnimator.OfFloat(mExpandedImageView, View.X, startBounds.Left, finalBounds.Left))
                .With(ObjectAnimator.OfFloat(mExpandedImageView, View.Y, startBounds.Top, finalBounds.Top))
                .With(ObjectAnimator.OfFloat(mExpandedImageView, View.ScaleXs, startScale, 1f))
                .With(ObjectAnimator.OfFloat(mExpandedImageView, View.ScaleYs, startScale, 1f));

            set.SetDuration(ShortAnimationDuration);
            set.AnimationEnd += (o, e) => currentAnimator = null;
            set.AnimationCancel += (o, e) => currentAnimator = null;
            set.Start();
            currentAnimator = set;

            set.SetInterpolator(new DecelerateInterpolator());

            // Upon clicking the zoomed-in image, it should zoom back down
            // to the original bounds and show the thumbnail instead of
            // the expanded image.
            var startScaleFinal = startScale;

            mExpandedImageView.Click += (o, e) =>
            {
                currentAnimator?.Cancel();
                mFrameContainer.SetBackgroundColor(Color.Transparent);

                // Animate the four positioning/sizing properties in parallel,
                // back to their original values.
                AnimatorSet setBack = new AnimatorSet();
                setBack.Play(ObjectAnimator.OfFloat(mExpandedImageView, View.X, startBounds.Left))
                        .With(ObjectAnimator.OfFloat(mExpandedImageView, View.Y, startBounds.Top))
                        .With(ObjectAnimator.OfFloat(mExpandedImageView, View.ScaleXs, startScaleFinal))
                        .With(ObjectAnimator.OfFloat(mExpandedImageView, View.ScaleYs, startScaleFinal));
                setBack.SetDuration(ShortAnimationDuration);
                setBack.SetInterpolator(new DecelerateInterpolator());
                setBack.AnimationEnd += (oo, ee) =>
                {
                    mThumbView.Alpha = 1f;
                    mExpandedImageView.Visibility = ViewStates.Gone;
                    currentAnimator = null;
                };
                setBack.AnimationCancel += (oo, ee) =>
                {
                    mThumbView.Alpha = 1f;
                    mExpandedImageView.Visibility = ViewStates.Gone;
                    currentAnimator = null;
                };
                setBack.Start();
                currentAnimator = setBack;
            };
        }
    }
}
