namespace Camera2DemoApp.Behaviours
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class EventToCommandBehaviour : BehaviourBase<VisualElement>
    {
        private Delegate? eventHandler;

        public static readonly BindableProperty EventNameProperty = BindableProperty.Create("EventName", typeof(string),
            typeof(EventToCommandBehaviour), propertyChanged: OnEventNameChanged)!;

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(EventToCommandBehaviour))!;

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create("CommandParameter", typeof(object), typeof(EventToCommandBehaviour))!;

        public static readonly BindableProperty InputConverterProperty =
            BindableProperty.Create("Converter", typeof(IValueConverter), typeof(EventToCommandBehaviour))!;

        public string EventName
        {
            get => (string)GetValue(EventNameProperty);
            set => SetValue(EventNameProperty, value);
        }

        public ICommand? Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object? CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            // set => SetValue(CommandParameterProperty, value);
        }

        public IValueConverter? Converter
        {
            get => (IValueConverter)GetValue(InputConverterProperty);
            set => SetValue(InputConverterProperty, value);
        }

        protected override void OnAttachedTo(VisualElement bindable)
        {
            base.OnAttachedTo(bindable);
            RegisterEvent(EventName);
        }

        protected override void OnDetachingFrom(VisualElement bindable)
        {
            DeregisterEvent(EventName);
            base.OnDetachingFrom(bindable);
        }

        void RegisterEvent(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            EventInfo? eventInfo = AssociatedObject?.GetType().GetRuntimeEvent(name);
            if (eventInfo == null)
            {
                throw new ArgumentException($"EventToCommandBehavior: Can't register the '{EventName}' event.");
            }

            MethodInfo? methodInfo = typeof(EventToCommandBehaviour).GetTypeInfo()?.GetDeclaredMethod(nameof(OnEvent));
            eventHandler = methodInfo?.CreateDelegate(eventInfo.EventHandlerType, this);
            eventInfo.AddEventHandler(AssociatedObject, eventHandler);
        }

        private void DeregisterEvent(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            if (eventHandler == null)
            {
                return;
            }

            EventInfo? eventInfo = AssociatedObject?.GetType().GetRuntimeEvent(name);
            if (eventInfo == null)
            {
                throw new ArgumentException($"EventToCommandBehavior: Can't de-register the '{EventName}' event.");
            }

            eventInfo.RemoveEventHandler(AssociatedObject, eventHandler);
            eventHandler = null;
        }

        private bool onEventJustRan;

        /// <summary>
        /// Just a little breaking of MVVM here - store the sender as a static that
        /// can be accessed from anywhere. This wouldn't be necessary except for the
        /// fact the switch really wants to toggle even if it's bound to a get-only
        /// that is returning it's opposite.
        /// </summary>
        public static object? EventSender;

        private void OnEvent(object sender, object eventArgs)
        {
            EventSender = null;
            if (onEventJustRan)
            {
                onEventJustRan = false;
                return;
            }

            if (Command == null)
            {
                return;
            }

            onEventJustRan = true;

            object resolvedParameter;
            if (CommandParameter != null)
            {
                resolvedParameter = CommandParameter;
            }
            else if (Converter != null)
            {
                resolvedParameter = Converter.Convert(eventArgs, typeof(object), null, null);
            }
            else
            {
                resolvedParameter = eventArgs;
            }

            if (Command.CanExecute(resolvedParameter))
            {
                EventSender = sender;
                Command.Execute(resolvedParameter);
            }
        }

        static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            string oldEventName = (string)oldValue;
            string newEventName = (string)newValue;

            var behavior = (EventToCommandBehaviour)bindable;
            if (behavior.AssociatedObject == null)
            {
                // wait up to 2 seconds for everything to init...
                Task.Run(async () =>
                {
                    var retries = 0;
                    while (behavior.AssociatedObject == null && retries < 10)
                    {
                        await Task.Delay(200);
                        retries++;
                    }

                    behavior.DeregisterEvent(oldEventName);
                    behavior.RegisterEvent(newEventName);
                });
                return;
            }

            behavior.DeregisterEvent(oldEventName);
            behavior.RegisterEvent(newEventName);
        }
    }
}
