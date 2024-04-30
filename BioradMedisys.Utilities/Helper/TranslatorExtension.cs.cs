using Coditech.DataAccessLayer.DataEntity;
using Coditech.Model;
using Coditech.Utilities.Filters;
using Coditech.ViewModel;

using System.Collections.Generic;

namespace Coditech.Utilities.Helper
{
    public static class TranslatorExtension
    {
        #region Entity
        /// <summary>
        /// Translate Model to Entity
        /// </summary>
        /// <typeparam name="TEntity">Translate Model to TEntity</typeparam>
        /// <param name="modelBase">BaseModel is extended class</param>
        /// <returns></returns>
        public static TEntity FromModelToEntity<TEntity>(this BaseModel modelBase)
            => Translator.Translate<TEntity>(modelBase);

        /// <summary>
        /// Transalate Entity to Model
        /// </summary>
        /// <typeparam name = "TModel" ></ typeparam >
        /// < param name="Entity"></param>
        /// <returns></returns>
        public static TModel FromEntityToModel<TModel>(this CoditechEntityBaseModel entity)
            => Translator.Translate<TModel>(entity);

        #endregion
        ///// <summary>
        ///// Translate Model to Entity
        ///// </summary>
        ///// <typeparam name="TEntity">TEntity is a destination model, having contraint TEntity is a RARIndiaEntityBaseModel</typeparam>
        ///// <typeparam name="TModel">TModel is source model</typeparam>
        ///// <param name="Model">model is extended class</param>
        ///// <returns></returns>
        //public static TEntity ToEntity<TEntity, TModel>(this TModel model) where TEntity : RARIndiaEntityBaseModel
        //    => Translator.Translate<TEntity, TModel>(model);

        ///// <summary>
        ///// Translate Model collection to Model Entity
        ///// </summary>
        ///// <typeparam name="TEntity">TEntity is a destination model</typeparam>
        ///// <param name="collection">collection is extended BaseModel class list</param>
        ///// <returns></returns>
        //public static IEnumerable<TEntity> ToEntity<TEntity>(this IEnumerable<BaseModel> collection)
        //    => Translator.Translate<TEntity>(collection);

        ///// <summary>
        ///// Translate Model collection to Model Entity
        ///// </summary>
        ///// <typeparam name="TEntity"></typeparam>
        ///// <typeparam name="TModel"></typeparam>
        ///// <param name="collection"></param>
        ///// <returns></returns>
        //public static IEnumerable<TEntity> ToEntity<TEntity, TModel>(this IEnumerable<TModel> collection)
        //    => Translator.Translate<TEntity, TModel>(collection);       



        /// <summary>
        /// Transalate Entity to Model
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public static TModel ToModel<TModel, TEntity>(this TEntity entity)
            => Translator.Translate<TModel, TEntity>(entity);

        /// <summary>
        /// Translate Collection Entity to Collection Model
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="EntityCollection"></param>
        /// <returns></returns>
        public static IEnumerable<TModel> ToModel<TModel>(this IEnumerable<CoditechEntityBaseModel> entityCollection)
            => Translator.Translate<TModel>(entityCollection);

        /// <summary>
        /// Translate Collection Entity to Collection Model
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="EntityCollection"></param>
        /// <returns></returns>
        public static IEnumerable<TModel> ToModel<TModel, TEntity>(this IEnumerable<TEntity> entityCollection)
            => Translator.Translate<TModel, TEntity>(entityCollection);

        /// <summary>
        /// Transalate View Model to Model
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static TModel ToModel<TModel>(this BaseViewModel viewModel)
            => Translator.Translate<TModel>(viewModel);
        /// <summary>
        /// Translate Model to ViewModel
        /// </summary>
        /// <typeparam name="TDTOModel">Translate Model to ViewModel</typeparam>
        /// <param name="modelBase">APIBaseModel is extended class</param>
        /// <returns></returns>
        public static TDTOModel ToViewModel<TDTOModel>(this BaseModel modelBase)
            => Translator.Translate<TDTOModel>(modelBase);

        /// <summary>
        /// Translate Model to ViewModel
        /// </summary>
        /// <typeparam name="TDTOModel">TDTOModel is a destination model, having contraint TDTOModel is a BaseModel</typeparam>
        /// <typeparam name="TModel">TModel is source model</typeparam>
        /// <param name="Model">model is extended class</param>
        /// <returns></returns>
        public static TDTOModel ToViewModel<TDTOModel, TModel>(this TModel model) where TDTOModel : BaseModel
            => Translator.Translate<TDTOModel, TModel>(model);

        /// <summary>
        /// Translate Model collection to View Model
        /// </summary>
        /// <typeparam name="TDTOModel">TDTOModel is a destination model</typeparam>
        /// <param name="collection">collection is extended APIBaseModel class list</param>
        /// <returns></returns>
        public static IEnumerable<TDTOModel> ToViewModel<TDTOModel>(this IEnumerable<BaseModel> collection)
            => Translator.Translate<TDTOModel>(collection);

        /// <summary>
        /// Translate Model collection to View Model
        /// </summary>
        /// <typeparam name="TDTOModel"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IEnumerable<TDTOModel> ToViewModel<TDTOModel, TModel>(this IEnumerable<TModel> collection)
            => Translator.Translate<TDTOModel, TModel>(collection);

        /// <summary>
        /// Translate Filter Collection to Filter Data Collection
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static FilterDataCollection ToFilterDataCollections(this FilterCollection filters)
        {
            FilterDataCollection dataCollection = new FilterDataCollection();
            if (!Equals(filters, null))
            {
                dataCollection.AddRange(filters.ToModel<FilterDataTuple, FilterTuple>());
            }
            return dataCollection;
        }
    }
}