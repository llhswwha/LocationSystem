using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Shapes;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Location.Data;
using WPFClientControlLib.Behaviors;

namespace WPFClientControlLib.AreaCanvaItems
{
    public class PersonShape
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public TagPosition Pos { get; set; }

        private AreaCanvas _canvas;

        private double _scale;

        private double _size;

        public PersonShape(AreaCanvas canvas, int id,string name, TagPosition pos, double scale, double size = 2)
        {
            
            Id = id;
            Name = name;
            _canvas = canvas;
            Pos = pos;
            _scale = scale;
            _size = size;
            if (pos != null)
            {
                PosX = Pos.X;
                PosY = Pos.Z;
            }              
        }

        public double PosX { get; set; }

        public double PosY { get; set; }

        private double GetLeft()
        {
            double left = (PosX - _canvas.OffsetX) * _scale - _size * _scale / 2;
            //double x = GetX(left);
            return left;
        }

        private double GetTop()
        {
            double top = (PosY - _canvas.OffsetY) * _scale - _size * _scale / 2;
            double y = GetY(top);
            return top;
        }

        private double GetX(double left)
        {
            double x=(left + _size*_scale/2)/_scale + _canvas.OffsetX;
            return x;
        }

        private double GetY(double top)
        {
            double y = (top + _size * _scale / 2) / _scale + _canvas.OffsetY;
            return y;
        }

        private Ellipse personShape;

        private double minSize = 10;
        private double maxSize = 40;

        public Ellipse Show()
        {
            _canvas.RemovePerson(Id);

            //人员的比例和位置的比例分开

            personShape = new Ellipse()
            {
                //Margin = new Thickness(x, y, 0, 0),
                Width = _size * _scale,
                Height = _size * _scale,
                Fill = Brushes.GreenYellow,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Tag = Id,
                ToolTip = Name
            };

           
            DragInCanvasBehavior behavior1 = new DragInCanvasBehavior();
            behavior1.Moved += Behavior1_Moved;
            Interaction.GetBehaviors(personShape).Add(behavior1);

            double left = GetLeft();
            double top = GetTop();

            if(personShape.Width< minSize)
                personShape.Width = minSize;
            if (personShape.Height < minSize)
                personShape.Height = minSize;

            if (personShape.Width > maxSize)
                personShape.Width = maxSize;
            if (personShape.Height > maxSize)
                personShape.Height = maxSize;

            //left = 0;
            //top = 0;

            Canvas.SetLeft(personShape, left);
            Canvas.SetTop(personShape, top);

            _canvas.AddPerson(Id, personShape);
            personShape.MouseDown += PersonShape_MouseDown;
            personShape.MouseEnter += PersonShape_MouseEnter;
            personShape.MouseLeave += PersonShape_MouseLeave;
            
            return personShape;
        }


        private void PersonShape_MouseLeave(object sender, MouseEventArgs e)
        {
            Ellipse rect = sender as Ellipse;
            if (SelectedPerson == rect) return;
            var dev = rect.Tag as Location.TModel.Location.Person.Personnel;
            rect.Fill = Brushes.GreenYellow;
            rect.Stroke = Brushes.Black;
        }

        private void PersonShape_MouseEnter(object sender, MouseEventArgs e)
        {
            //SelectRectangle(sender as Rectangle);
            Ellipse rect = sender as Ellipse;
            var person = rect.Tag as Location.TModel.Location.Person.Personnel;

            rect.Fill = Brushes.Blue;
            rect.Stroke = Brushes.Red;
        }

        public event Action<Ellipse, Location.TModel.Location.Person.Personnel> PersonSelected;

        public static Ellipse SelectedPerson;

        private void PersonShape_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedPerson != null)
            {
                SelectedPerson.Fill = Brushes.GreenYellow;
                SelectedPerson.Stroke = Brushes.Black;
            }
            Ellipse rect = sender as Ellipse;
            var person = rect.Tag as Location.TModel.Location.Person.Personnel;
            SelectedPerson = rect;

            if (PersonSelected != null)
            {
                PersonSelected(rect, person);
            }
        }

        private void Behavior1_Moved(object arg1, System.Windows.Point arg2)
        {
            double left = Canvas.GetLeft(personShape);
            double top = Canvas.GetTop(personShape);

            double x = GetX(left);
            double y = GetY(top);

            PosX = x;
            PosY = y;

            //this.XPos = left;
            //this.YPos = top;
            //Position.XPos = (int)left;
            //Position.YPos = (int)top;

            OnMoved();
        }

        public TagPosition SavePos()
        {
            Pos.X= (float)PosX;
            Pos.Z = (float)PosY;
            return Pos;
        }

        public event Action<PersonShape> Moved;

        private void OnMoved()
        {
            if (Moved != null)
            {
                Moved(this);
            }
        }
    }
}
