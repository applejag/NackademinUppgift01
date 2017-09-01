using ConsoleDrawing.Structs;
using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDrawing.Objects
{
    public abstract class Drawable
    {
        public static readonly HashSet<Drawable> all = new HashSet<Drawable>();

        private bool m_Initiated = false;
        private bool m_Destroyed = false;
        private bool m_Enabled = true;
        private Drawable m_Parent = null;

        /// <summary>
        /// Gets or sets this objects enabled state.
        /// Disabled object do not receive <seealso cref="Update"/> or <seealso cref="Draw"/> events.
        /// <para>Setting is identical to <see cref="SetEnabled"/></para>
        /// </summary>
        public bool Enabled {
            get => m_Enabled && !m_Destroyed;
            set => SetEnabled(value);
        }

        /// <summary>
        /// Wether or not this object has been destroyed using <see cref="Destroy"/>.
        /// </summary>
        public bool Destroyed => m_Destroyed;

        /// <summary>
        /// Set or get this objects parent.
        /// <para>Setting is identical to <see cref="SetParent"/></para>
        /// </summary>
        public Drawable Parent
        {
            get => m_Parent;
            set => SetParent(value);
        }
        
        private readonly List<Drawable> children = new List<Drawable>();

        /// <summary>
        /// Local position of this object.
        /// </summary>
        public Vector2 localPosition;
        
        /// <summary>
        /// Position relative to this objects parent (if any)
        /// </summary>
        public Vector2 Position {
            get => Parent == null ? localPosition : (Parent.Position + localPosition);
            set => localPosition = Parent == null ? value : (value - Parent.Position);
        }
        
        /// <summary>
        /// Same as <see cref="Position"/>, but with <seealso cref="int"/> instead of <seealso cref="float"/>.
        /// </summary>
        public Point ApproxPosition => (Point)Position;

        public Drawable()
        {
            SetEnabled(true);
        }

        ~Drawable()
        {
            Destroy();
        }

        /// <summary>
        /// Sets this objects parent.
        /// </summary>
        /// <param name="parent">The new parent. Can be null to set no parent.</param>
        /// <param name="worldPositionStays">If true, the local position is modified to retain the original "world" position.</param>
        public void SetParent(Drawable parent, bool worldPositionStays = true)
        {
            if (m_Parent == parent) return;
            if (m_Destroyed) return;

            Vector2 oldPos = Position;
            
            // Remove from old parent
            m_Parent?.children.Remove(this);

            // Add to new parent
            parent?.children.Add(this);

            if (worldPositionStays)
            {
                Position = oldPos;
            }
        }

        /// <summary>
        /// Enables/disables this object.
        /// Disabled object do not receive <seealso cref="Update"/> or <seealso cref="Draw"/> events.
        /// </summary>
        public void SetEnabled(bool state)
        {
            if (m_Destroyed) return;
            if (state)
            {
                Time.OnEventUpdate += Update;
                Time.OnEventDraw += Draw;

                if (!m_Initiated)
                {
                    m_Initiated = true;
                    all.Add(this);
                }
            }
            else
            {
                Time.OnEventUpdate -= Update;
                Time.OnEventDraw -= Draw;
            }
            m_Enabled = state;
        }
        
        public abstract void Update();
        public abstract void Draw();

        /// <summary>
        /// Destroy this drawable. Makes it inactive.
        /// </summary>
        public void Destroy()
        {
            if (Destroyed) return;
            
            Time.OnEventUpdate -= Update;
            Time.OnEventDraw -= Draw;
            SetEnabled(false);
            m_Destroyed = true;

            // Remove all children
            foreach (var child in children)
            {
                child?.Destroy();
            }

            SetParent(null);

            if (m_Initiated)
                all.Remove(this);
        }

        /// <summary>
        /// Destroy this drawable after a <paramref name="delay"/>. Makes it inactive.
        /// </summary>
        /// <param name="delay">The delay in seconds.</param>
        public async void Destroy(float delay)
        {
            await Task.Delay((int)(delay * 1000));
            Destroy();
        }
    }
}
