using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using RecordLabel.Content;
using System.Linq.Expressions;

namespace RecordLabel.Web.Controllers
{
    /// <summary>
    /// Base controller with database context for a individual types
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class EntityBaseController<TModel> : BaseController where TModel : Base
    {
        private ReleaseContext db;
        private DbSet<TModel> entitySet;
        public DbSet<TModel> EntitySet => entitySet;

        protected int ItemsPerPage { get; set; }

        protected delegate IQueryable<TModel> OrderByDelegate(IQueryable<TModel> initialQuery);

        /// <summary>
        /// Function for ordering a supplied query
        /// </summary>
        protected OrderByDelegate ModelOrderBy { get; set; }

        /// <summary>
        /// Used to query a list of models from a specified query. Supposed take care of necessary Include and other statements
        /// </summary>
        protected Func<IQueryable<TModel>, IQueryable<TModel>> ListModelQuery { get; set; }

        /// <summary>
        /// Returns query that uses ListModelQuery to query the supplied query a specified batch of models while ordering them using ModelOrderBy expression
        /// </summary>
        protected Func<IQueryable<TModel>, int, IQueryable<TModel>> BatchedListModelQuery => (initQuery, batch) => ModelOrderBy(ListModelQuery(initQuery)).Skip(batch * ItemsPerPage).Take(ItemsPerPage);  //OrderBy(ListModelQuery(initQuery)).Skip(batch * itemsPerPage).Take(itemsPerPage);

        /// <summary>
        /// Returns a complete query for a model to be viewed or edited 
        /// </summary>
        protected Func<IQueryable<TModel>, IQueryable<TModel>> CompleteModelQuery { get; set; }

        public EntityBaseController(ReleaseContext dbContext, Func<ReleaseContext, DbSet<TModel>> entitySet) : base (dbContext)
        {
            db = dbContext;
            this.entitySet = entitySet(DbContext);
            ModelOrderBy = init => init.OrderBy(item => item.Id);
            ListModelQuery = initQuery => initQuery;
            CompleteModelQuery = initQuery => ListModelQuery(initQuery);

            ItemsPerPage = Settings.ListItemBatchSize;
        }

        [ProcessTempDataError]
        public override ActionResult Index()
        {
            return List();
        }

        public virtual ActionResult List()
        {
            ViewBag.Title = typeof(TModel).Name;
            return View(IndexViewName, SelectModels(0));
        }

        public virtual ActionResult View(int id)
        {
            TModel entity = SelectModel(id);
            PrepareViewBagForView(entity);
            return View("View", entity);
        }

        [Authorize]
        public virtual ActionResult Create()
        {
            PrepareViewBagForCreate();
            return View(EditViewName);
        }

        /// <summary>
        /// Loads Edit Model page. Calls SelectModel, checks if entity is not empty, then calls PrepareViewBagForEdit and returns an Edit view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public virtual ActionResult Edit(int id)
        {
            TModel model = SelectModel(id);
            
            if (EntityExists(model))
            {
                PrepareViewBagForEdit(model);
                return View(EditViewName, model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(TModel postedModel)
        {
            return SavePostedModel(postedModel, modelPosted => EntitySet.Add(modelPosted));//(model, modelPosted) => EntitySet.Add(model));
        }

        delegate void AddChangesToDbContext(TModel postedModel);

        [HttpPost]
        [Authorize]
        public virtual ActionResult Edit(TModel postedModel)
        {
            return SavePostedModel(postedModel, modelPosted =>
            {
                TModel model = SelectModel(postedModel.Id);
                DbContext.UpdateModel(model, postedModel);
            });
        }

        /// <summary>
        /// Performs model validation and saving to the database and returns an appropriate view (List in case of success, Edit in case model validation of error)
        /// </summary>
        /// <param name="postedModel"></param>
        /// <param name="saveToDbAction">Action that updates an existing model </param>
        /// <returns></returns>
        private ActionResult SavePostedModel(TModel postedModel, AddChangesToDbContext addChangesToDbContext)
        {
            OnModelValidation(postedModel);
            if (!ModelState.IsValid)
            {
                TModel model = SelectModel(postedModel.Id);
                model.UpdateModel(DbContext, postedModel);
                PrepareViewBagForEdit(model);
                return View(EditViewName, postedModel);
            }

            addChangesToDbContext(postedModel);

            DbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public virtual ActionResult Delete(int id)
        {
            TModel model = SelectModelForDeletion(id);

            if (EntityExists(model))
            {
                OnModelDelete(model);
                DbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Selects a batch of models for displaying in a list from the database using BatchedListModelQuery
        /// </summary>
        /// <param name="batch"></param>
        /// <returns></returns>
        protected virtual TModel[] SelectModels(int batch)
        {
            return SelectModels(batch, null);
        }

        /// <summary>
        /// Selects a batch of models for displaying in a list from the database using BatchedListModelQuery and an extra filter
        /// </summary>
        /// <param name="batch">Batch number (begins with 0)</param>
        /// <param name="filter">Filter that is applied selecting models from the database before breaking them into batches</param>
        /// <returns></returns>
        protected virtual TModel[] SelectModels(int batch, Expression<Func<TModel, bool>> filter)
        {
            IQueryable<TModel> initialQuery;
            if (filter != null)
            {
                initialQuery = EntitySet.Where(filter);
            }
            else
            {
                initialQuery = EntitySet;
            }

            TModel[] models = BatchedListModelQuery(initialQuery, batch).ToArray(); // QueryModelsInBatches(initialQuery, batch).ToArray();
            ViewBag.ItemCount = initialQuery.Count();
            return models;
        }

        /// <summary>
        /// Selects a single model from the database and loads all properties using CompleteModelQuery
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual TModel SelectModel(int id)
        {
            return CompleteModelQuery(EntitySet).SingleOrDefault(entity => entity.Id == id);
        }

        protected virtual TModel SelectModelForDeletion(int id)
        {
            return SelectModel(id);
        }

        /// <summary>
        /// Checks if a model is null, and if it is, adds "Error" to TempData
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected virtual bool EntityExists(TModel model)
        {
            if (model == null)
            {
                TempData.Add("Error", $"Item with id {ModelState["Id"].Value.RawValue} not found");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Overriding methods should performa additional model validations here
        /// </summary>
        /// <param name="postedModel"></param>
        protected virtual void OnModelValidation(TModel postedModel)
        {
            
        }

        protected virtual void OnModelDelete(TModel model)
        {
            model.Delete(DbContext);
        }

        protected virtual void PrepareViewBagForCreate()
        {

        }
        protected virtual void PrepareViewBagForEdit(TModel model)
        {

        }

        protected virtual void PrepareViewBagForView(TModel model)
        {
           
        }
    }
}