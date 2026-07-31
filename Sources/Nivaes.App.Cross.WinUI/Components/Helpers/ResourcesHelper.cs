using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace Nivaes.App.Cross.WinUI
{
    public static class ResourcesHelper
    {
        public static string GetString(string resourceName)
        {
            return (string)Application.Current.Resources[resourceName];
        }

        public static IconElement? GetIcon(string resourceName)
        {
            try
            {
                var icon = (PathIcon)Application.Current.Resources[resourceName];
                var source = (PathGeometry)icon.Data;

                return new PathIcon
                {
                    Data = ClonePathGeometry(source)
                };
            }
            catch(Exception ex)
            {
                throw new AppException(ex, $"Icon {resourceName} does not exist");
            }
        }

        private static PathGeometry ClonePathGeometry(PathGeometry source)
        {
            var geometry = new PathGeometry { FillRule = source.FillRule };

            foreach (var figure in source.Figures)
            {
                var newFigure = new PathFigure
                {
                    StartPoint = figure.StartPoint,
                    IsClosed = figure.IsClosed,
                    IsFilled = figure.IsFilled
                };

                foreach (var segment in figure.Segments)
                {
                    switch (segment)
                    {
                        case LineSegment line:
                            newFigure.Segments.Add(new LineSegment { Point = line.Point });
                            break;
                        case BezierSegment bezier:
                            newFigure.Segments.Add(new BezierSegment
                            {
                                Point1 = bezier.Point1,
                                Point2 = bezier.Point2,
                                Point3 = bezier.Point3
                            });
                            break;
                        case ArcSegment arc:
                            newFigure.Segments.Add(new ArcSegment
                            {
                                Point = arc.Point,
                                Size = arc.Size,
                                RotationAngle = arc.RotationAngle,
                                IsLargeArc = arc.IsLargeArc,
                                SweepDirection = arc.SweepDirection
                            });
                            break;
                        case PolyLineSegment poly:
                            var newPoly = new PolyLineSegment();
                            foreach (var pt in poly.Points) newPoly.Points.Add(pt);
                            newFigure.Segments.Add(newPoly);
                            break;
                        case QuadraticBezierSegment quad:
                            newFigure.Segments.Add(new QuadraticBezierSegment
                            {
                                Point1 = quad.Point1,
                                Point2 = quad.Point2
                            });
                            break;
                    }
                }

                geometry.Figures.Add(newFigure);
            }

            return geometry;
        }

        public static Canvas GetCanvas(string resourceName)
        {
            return (Canvas)Application.Current.Resources[resourceName];
        }

        public static DataTemplate GetDataTemplate(string resourceName)
        {
            return (DataTemplate)Application.Current.Resources[resourceName];
        }

        public static DependencyObject GetDependencyObject(string resourceName)
        {
            return (DependencyObject)Application.Current.Resources[resourceName];
        }
    }
}
