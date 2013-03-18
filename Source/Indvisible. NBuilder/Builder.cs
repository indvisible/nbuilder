namespace Indvisible.NBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FizzWare.NBuilder;
    using FizzWare.NBuilder.Implementation;
    using FizzWare.NBuilder.PropertyNaming;

    public class BuilderNested
    {
        public static IObjectBuilder CreateNew(Type type)
        {
            var reflectionUtil = new ReflectionUtil();
            var propertyNamer = BuilderSetup.GetPropertyNamerFor(type);
            return new ObjectBuilder(type, reflectionUtil).WithPropertyNamer(propertyNamer);
        }

        public static IObjectBuilder CreateNew(Type type, IPropertyNamer propertyNamer)
        {
            return new ObjectBuilder(type, new ReflectionUtil()).WithPropertyNamer(propertyNamer);
        }

        //public static IListBuilder CreateListOfSize(Type type, int size)
        //{
        //    Guard.Against(size < 1, "Size of list must be 1 or greater");
        //    var propertyNamer = BuilderSetup.GetPropertyNamerFor(type);
        //    return CreateListOfSize(size, propertyNamer);
        //}

        //public static IListBuilder CreateListOfSize(Type type, int size, IPropertyNamer propertyNamer)
        //{
        //    return new ListBuilder(type, size, propertyNamer, new ReflectionUtil());
        //}
    }

    public interface IListBuilder : IBuildable
    {
    }

    public interface IQueue
    {
        void Enqueue(dynamic item);
        dynamic Dequeue();
        dynamic GetLastItem();
        int Count { get; }
    }

    public interface IDeclarationQueue<T> : IQueue<IDeclaration<T>>
    {
        void Construct();
        void Prioritise();
        bool ContainsGlobalDeclaration();
    }

//    public class ListBuilder : IListBuilderImpl
//    {
//        private readonly int size;
//        private readonly IPropertyNamer propertyNamer;
//        private readonly IReflectionUtil reflectionUtil;
//        private readonly dynamic[] mainList;
//        private readonly DeclarationQueue<dynamic> declarations;

//        public virtual int Capacity
//        {
//            get
//            {
//                return size;
//            }
//        }

//        public IDeclarationQueue<dynamic> Declarations
//        {
//            get { return declarations; }
//        }

//        public ListBuilder(int size, IPropertyNamer propertyNamer, IReflectionUtil reflectionUtil)
//        {
//            this.size = size;
//            this.propertyNamer = propertyNamer;
//            this.reflectionUtil = reflectionUtil;

//            mainList = new dynamic[size];

//            declarations = new DeclarationQueue<dynamic>(size);

//            ScopeUniqueRandomGenerator = new UniqueRandomGenerator();
//        }

//        public IObjectBuilder<dynamic> CreateObjectBuilder()
//        {
//            return new ObjectBuilder(reflectionUtil);
//        }

//#if OBSOLETE_OLD_SYNTAX
//        [Obsolete(Messages.NewSyntax_UseAll)]
//#endif
//        public IOperable<T> WhereAll()
//        {
//            return All();
//        }

//        public IOperable<T> All()
//        {
//            declarations.Enqueue(new GlobalDeclaration<T>(this, CreateObjectBuilder()));
//            return (IOperable<T>)declarations.GetLastItem();
//        }

//        public void Construct()
//        {
//            if (declarations.GetDistinctAffectedItemCount() < this.Capacity &&
//                !declarations.ContainsGlobalDeclaration() &&
//                reflectionUtil.RequiresConstructorArgs(typeof(T))
//                )
//            {
//                throw new BuilderException(
//                    @"The type requires constructor args but they have not be supplied for all the elements of the list");
//            }

//            if (declarations.GetDistinctAffectedItemCount() < this.Capacity && !declarations.ContainsGlobalDeclaration())
//            {
//                var globalDeclaration = new GlobalDeclaration<T>(this, CreateObjectBuilder());
//                declarations.Enqueue(globalDeclaration);
//            }

//            declarations.Construct();
//        }

//        public IList<T> Name(IList<T> list)
//        {
//            if (!BuilderSetup.AutoNameProperties)
//                return list;

//            propertyNamer.SetValuesOfAllIn(list);
//            return list;
//        }

//        public IList<T> Build()
//        {
//            Construct();
//            declarations.AddToMaster(mainList);

//            var list = mainList.ToList();

//            Name(list);
//            declarations.CallFunctions(list);

//            return list;
//        }

//        public IDeclaration<T> AddDeclaration(IDeclaration<T> declaration)
//        {
//            this.declarations.Enqueue(declaration);
//            return declarations.GetLastItem();
//        }

//        public IUniqueRandomGenerator ScopeUniqueRandomGenerator
//        {
//            get;
//            private set;
//        }
//    }
}