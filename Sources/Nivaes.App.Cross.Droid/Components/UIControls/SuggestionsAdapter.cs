using System.Globalization;
using Android.Content;
using Android.Database;
using Android.Graphics;
using Android.Views;
using CursorAdapter = AndroidX.CursorAdapter.Widget.CursorAdapter;

namespace Nivaes.App.Cross.Droid
{
    public class SuggestionsAdapter
        : CursorAdapter
    {
        public SuggestionsAdapter(Context context, IEnumerable<string> values)
            : base(context, new MatrixCursor(new string[] { "_id", SearchManager.SuggestColumnText1 }), 0)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

            Java.Lang.Object func(string s) => new Java.Lang.String(s);

            var curtor = (MatrixCursor)base.Cursor;
            int n = 0;
            var enumerator = values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                curtor.AddRow(Array.ConvertAll<string, Java.Lang.Object>(new string[] { (n++).ToString(CultureInfo.CurrentCulture), enumerator.Current }, func));
            }
        }

        public override View NewView(Context context, ICursor cursor, ViewGroup parent)
        {
            LayoutInflater inflater = LayoutInflater.From(context);
            return inflater.Inflate(Android.Resource.Layout.SimpleListItem1, parent, false);
        }

        public override void BindView(View view, Context context, ICursor cursor)
        {
            ArgumentNullException.ThrowIfNull(view);
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(cursor);

            Android.Widget.TextView tv = (Android.Widget.TextView)view;
            int textIndex = cursor.GetColumnIndex(SearchManager.SuggestColumnText1);
            tv.Text = cursor.GetString(textIndex);

            tv.SetTextColor(Color.Black);
            tv.SetBackgroundColor(Color.White);
        }

        public string GetValue()
        {
            int textIndex = base.Cursor.GetColumnIndex(SearchManager.SuggestColumnText1);
            return base.Cursor.GetString(textIndex);
        }
    }
}
