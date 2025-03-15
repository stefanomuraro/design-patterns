using System.Text;

namespace DesignPatterns
{
    class Program
    {
        static void Main(string[] args)
        {
            // Singleton
            var a = Singleton.Instance();
            a.Name = "sape";
            Console.WriteLine(a.Name);

            var b = Singleton.Instance();
            Console.WriteLine(b.Name);

            // Prototype
            var p1 = new Person()
            {
                Name = "Pepe",
                Age = 12
            };

            var p2 = p1.Clone() as Person;
            p2.Name = "Lolo";

            Console.WriteLine(p1.Name);

            // Adapter
            var adaptee = new Adaptee();
            ITarget target = new Adapter(adaptee);
            Console.WriteLine(target.Request());

            // Decorator
            var car = new Car();
            var offer = new SpecialOffer(car);
            offer.DiscountPercentage = 30;
            offer.Offer = "30% OFF";
            Console.WriteLine($"{offer.Offer} on {offer.Type} cars, new price is: ${offer.Price}");

            // State
            var stateClient = new StateContext(new ConcreteState1());
            stateClient.Request();
            stateClient.Request();
            stateClient.Request();

            // Strategy
            var strategyClient = new StrategyContext();
            strategyClient.SetStrategy(new ConcreteStrategy1());
            strategyClient.DoSomeBusinessLogic();
            strategyClient.SetStrategy(new ConcreteStrategy2());
            strategyClient.DoSomeBusinessLogic();
        }

        #region Creational design patterns

        // Singleton
        public class Singleton
        {
            private static Singleton _instance;

            public string Name { get; set; }

            protected Singleton() { }

            public static Singleton Instance()
            {
                if (_instance is null)
                    _instance = new Singleton();

                return _instance;
            }
        }

        // Prototype
        public class Person : ICloneable
        {
            public string Name { get; set; }
            public int Age { get; set; }

            public object Clone()
            {
                return this.MemberwiseClone();
            }
        }

        #endregion

        #region Structural design patterns

        // Adapter
        // To convert one interface into another interface.
        // To allow classes with incompatible interfaces to work together. It wraps an exisitng class with new interface.
        // To match an old component to a new system.
        class Adaptee
        {
            public string GetRequest()
            {
                return "Request from the client";
            }
        }

        interface ITarget // Client interface: describes how clients communicate with the service.
        {
            string Request();
        }

        class Adapter : ITarget
        {
            private readonly Adaptee _adaptee;

            public Adapter(Adaptee adaptee)
            {
                _adaptee = adaptee;
            }

            public string Request()
            {
                return $"This is the '{_adaptee.GetRequest()}'";
            }
        }

        // Decorator
        // To add new functionality to an existing object without altering its structure.
        // Acts as a wrapper to an existing class that lets you attach new behaviours to objects.
        // To assign additional behaviours to an object at runtime without breaking the existing code that uses those objects.
        // When it is awkward or impossible to extend an object's behaviour using inheritance.
        interface ICar
        {
            string Type { get; }
            double Price { get; }
        }

        class Car : ICar
        {
            public string Type => "Tesla";

            public double Price => 1000;
        }

        abstract class VehicleDecorator : ICar
        {
            private ICar _car;

            public VehicleDecorator(ICar car)
            {
                _car = car;
            }

            public string Type => _car.Type;

            public double Price => _car.Price;
        }

        class SpecialOffer : VehicleDecorator // Wrapper object with new behaviours
        {
            public string Offer { get; set; }

            public int DiscountPercentage { get; set; }

            public double Price => Math.Round(base.Price * (100 - DiscountPercentage) / 100, 2);

            public SpecialOffer(ICar car) : base(car) { }
        }

        #endregion

        #region Behavioural design patterns

        // State
        // When an object behaves differently depending on its current state.
        // If number of states is enormous, we avoid big nested switch statements and respect the OCP when want to add new states.
        public class StateContext
        {
            public IState State { get; set; }

            public StateContext(IState newState)
            {
                State = newState;
            }

            public void Request()
            {
                State.Handle(this);
            }
        }

        public interface IState
        {
            void Handle(StateContext context);
        }

        public class ConcreteState1 : IState
        {
            public void Handle(StateContext context)
            {
                Console.WriteLine("State 1 action.");
                context.State = new ConcreteState2();
            }
        }

        public class ConcreteState2 : IState
        {
            public void Handle(StateContext context)
            {
                Console.WriteLine("State 2 action.");
                context.State = new ConcreteState1();
            }
        }

        // Strategy
        // To switch the behaviour of an object (from one algorithm to another) during runtime.
        // To isolate business logic from algorithms implementation.
        // To avoid massive contidional operator (e.g. switch statements) and respect the OCP when want to add new variants of the algorithm.
        // Closely related to the notion of DI. The strategy gets injected into a class via constructor, prop or method.
        class StrategyContext
        {
            private IStrategy _strategy;

            public void SetStrategy(IStrategy strategy)
            {
                _strategy = strategy;
            }

            public void DoSomeBusinessLogic()
            {
                var result = _strategy.DoAlgorithm(new List<int> { 3, 1, 5, 2, 4 });
                var message = new StringBuilder();
                foreach (var item in result as List<int>)
                {
                    message.Append(item.ToString() + ", ");
                }
                Console.WriteLine(message.ToString());
            }
        }

        public interface IStrategy
        {
            object DoAlgorithm(object data);
        }

        class ConcreteStrategy1 : IStrategy
        {
            public object DoAlgorithm(object data)
            {
                var list = data as List<int>;
                list.Sort();
                return list;
            }
        }

        class ConcreteStrategy2 : IStrategy
        {
            public object DoAlgorithm(object data)
            {
                var list = data as List<int>;
                list.Sort();
                list.Reverse();
                return list;
            }
        }

        #endregion
    }

}
