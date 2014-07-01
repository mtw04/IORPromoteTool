using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Ninject;
using Ninject.Syntax;

using IORPromoteTool.IORPromoteEntities;

namespace IORPromoteTool.Infrastructure
{
    /// <summary>
    /// A base class for ninject dependency resolving.
    /// </summary>
    public class NinjectDependencyResolver : System.Web.Mvc.IDependencyResolver, IDependencyScope, System.Web.Http.Dependencies.IDependencyResolver
    {
        /// <summary> The ninject kernel. </summary>
        private IKernel kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectDependencyResolver" /> class.
        /// </summary>
        public NinjectDependencyResolver()
        {
            this.kernel = new StandardKernel();
        }

        /// <summary>
        /// Gets the kernel.
        /// </summary>
        public IKernel Kernel
        {
            get { return this.kernel; }
        }

        /// <summary>
        /// Starts a resolution scope.
        /// </summary>
        /// <returns>
        /// The dependency scope.
        /// </returns>
        public IDependencyScope BeginScope()
        {
            return new DependencyScope(this.kernel.BeginBlock());
        }

        /// <summary>
        /// This allows us to add bindings from outside of the class.
        /// </summary>
        /// <typeparam name="T"> The type of object to bind to. </typeparam>
        /// <returns> The function that allows binding outside of the class. </returns>
        public IBindingToSyntax<T> Bind<T>()
        {
            return this.kernel.Bind<T>();
        }

        /// <summary>
        /// Dispose of the kernel.
        /// </summary>
        public void Dispose()
        {
            this.kernel.Dispose();
        }

        /// <summary>
        /// Called by MVC when it requires a new instance of a class.
        /// </summary>
        /// <param name="serviceType"> Type of service. </param>
        /// <returns> The object that was requested. </returns>
        public object GetService(Type serviceType)
        {
            return this.kernel.TryGet(serviceType);
        }

        /// <summary>
        /// Called by MVC when it requires a new instance of a class.
        /// </summary>
        /// <param name="serviceType"> The type of service. </param>
        /// <returns> An enumerable of the objects requested. </returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.kernel.GetAll(serviceType);
        }

        /// <summary>
        /// Initializes the types.
        /// </summary>
        public virtual void InitializeTypes()
        {
            // Bind Services.
            this.kernel.Bind<IApplicationLogger>().ToConstant(new EnterpriseLogger());

            // Unit of work and Repositores

            // YOU ARE HERE!!!
            // Will need to move PromoteEntity to IORPromoteEntities and convert calls correctly



            //this.kernel.Bind<IDevPromoteEntities>().To<DevPromoteEntities>().WithConstructorArgument("connection", x => EntityConnectionModel.BuildEntityConnection());
            //this.kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
            //this.kernel.Bind(typeof(IGenericRepository<>)).To(typeof(GenericRepository<>));

            //// Services
            //this.kernel.Bind<IProfileImageService>().To<ProfileImageService>();
            //this.kernel.Bind<ITagService>().To<TagService>();

            //this.kernel.Bind<ITopicCategoryService>().To<TopicCategoryService>();
            //this.kernel.Bind<IPostService>().To<PostService>();
            //this.kernel.Bind<ISiteService>().To<SiteService>();
            //this.kernel.Bind<IUserService>().To<UserService>();
            //this.kernel.Bind<IReputationService>().To<ReputationService>();

            //this.kernel.Bind<Answers.Models.IAuthenticationHelper>().To<Answers.Models.AuthenticationHelper>();
        }

        /// <summary>
        /// Initializes the constants.
        /// </summary>
        public virtual void InitializeConstants()
        {
            // Initialize File Based - User Property repository.
        }

        /// <summary>
        /// A dependency scope.
        /// </summary>
        private class DependencyScope : IDependencyScope
        {
            /// <summary>
            /// A resolver for resolutions.
            /// </summary>
            private IResolutionRoot resolver;

            /// <summary>
            /// Initializes a new instance of the <see cref="DependencyScope"/> class.
            /// </summary>
            /// <param name="resolver">The resolver.</param>
            internal DependencyScope(IResolutionRoot resolver)
            {
                if (resolver == null)
                {
                    throw new ArgumentNullException("contract");
                }

                this.resolver = resolver;
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                IDisposable disposable = this.resolver as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }

                this.resolver = null;
            }

            /// <summary>
            /// Retrieves a service from the scope.
            /// </summary>
            /// <param name="serviceType">The service to be retrieved.</param>
            /// <returns>
            /// The retrieved service.
            /// </returns>
            public object GetService(Type serviceType)
            {
                if (this.resolver == null)
                {
                    throw new ObjectDisposedException("this", "This scope has already been disposed");
                }

                return this.resolver.TryGet(serviceType);
            }

            /// <summary>
            /// Retrieves a collection of services from the scope.
            /// </summary>
            /// <param name="serviceType">The collection of services to be retrieved.</param>
            /// <returns>
            /// The retrieved collection of services.
            /// </returns>
            public IEnumerable<object> GetServices(Type serviceType)
            {
                if (this.resolver == null)
                {
                    throw new ObjectDisposedException("this", "This scope has already been disposed");
                }

                return this.resolver.GetAll(serviceType);
            }
        }
    }
}