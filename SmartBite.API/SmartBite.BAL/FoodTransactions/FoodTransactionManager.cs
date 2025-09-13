using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBite.BAL.DTOS;
using SmartBite.DAL.Food;
using SmartBite.DAL.Models;

namespace SmartBite.BAL.FoodTransactions
{
    public class FoodTransactionManager : IFoodTransactionManager
    {
        private readonly IFoodService _service;

        public FoodTransactionManager(IFoodService service)
        {
            _service = service;
        }

        private static ItemNutrientModel _PrepareNewNutrient(ItemNutrientModel nutrient, decimal AmountInNewServing)
        {
            ItemNutrientModel NewNutrient = new ItemNutrientModel
            {
                Name = nutrient.Name,
                AmountPerServing = AmountInNewServing,
                AmountUnit = nutrient.AmountUnit
            };
            return NewNutrient;
        }

        private static decimal _CalculateNutrientAmountInNewServing(decimal OldServing, decimal NutrientAmountPerServing, decimal NewServing)
        {
            return (NutrientAmountPerServing * NewServing) / OldServing;
        }

        private static List<ItemNutrientModel> _GetUpdatedNutrients(FoodItemModel defaultItem, decimal EditedServing)
        {
            List<ItemNutrientModel> EditedNutrients = new List<ItemNutrientModel>();
            foreach (ItemNutrientModel nutrient in defaultItem.Nutrients!)
            {
                decimal AmountInNewServing = _CalculateNutrientAmountInNewServing(defaultItem.ServingSize, nutrient.AmountPerServing, EditedServing);
                EditedNutrients.Add(_PrepareNewNutrient(nutrient, AmountInNewServing));
            }
            return EditedNutrients;
        }

        private static decimal _GetEnergyForEditedItem(FoodItemModel DefaultItem, decimal EditedServing)
        {
            return (DefaultItem.EnergyPerServing * EditedServing) / DefaultItem.ServingSize;
        }

        private static FoodItemModel _GetItemChangesAfterEdit(FoodItemModel DefaultItem, decimal EditedServing)
        {
            FoodItemModel ItemAfterEdit = new FoodItemModel();
            ItemAfterEdit.Name = DefaultItem.Name;
            ItemAfterEdit.ServingSize = EditedServing;
            ItemAfterEdit.ServingUnit = DefaultItem.ServingUnit;
            ItemAfterEdit.Nutrients = _GetUpdatedNutrients(DefaultItem, EditedServing);
            ItemAfterEdit.EnergyPerServing = _GetEnergyForEditedItem(DefaultItem, EditedServing);
            return ItemAfterEdit;
        }

        private TinyFoodItemModel _GetTinyItemFromString(string RecognizedItem)
        {
            TinyFoodItemModel item = new TinyFoodItemModel();
            item = _service.GetTinyItemByName(RecognizedItem);
            return item;
        }

        private FoodItemModel _GetFoodItemWithEditedSize(TinyFoodItemDTO EditedtinyFoodItem)
        {
            FoodItemModel item = _service.GetFoodItemByName(EditedtinyFoodItem.Name!);
            return _GetItemChangesAfterEdit(item, EditedtinyFoodItem.ServingSize);
        }

        public List<TinyFoodItemModel> GetTinyItemsFromStringList(List<string> RecognizedItems)
        {
            List<TinyFoodItemModel> tinyFoodItems = new List<TinyFoodItemModel>();
            foreach (string RecognizedItem in RecognizedItems)
            {
                tinyFoodItems.Add(_GetTinyItemFromString(RecognizedItem));
            }
            return tinyFoodItems;
        }

        public List<FoodItemModel> GetFoodItemsWithEditedSize(List<TinyFoodItemDTO> EditedtinyFoodItems)
        {
            List<FoodItemModel> FoodItems = new List<FoodItemModel>();

            foreach (TinyFoodItemDTO EditedtinyItem in EditedtinyFoodItems)
            {
                FoodItemModel? item = _GetFoodItemWithEditedSize(EditedtinyItem);
                if (item != null)
                {
                    FoodItems.Add(item);
                }
            }

            return FoodItems;
        }


        public List<string> GetAllFoodItemsNames()
        {
            return _service.GetAllFoodItemsNames();
        }


    }
}