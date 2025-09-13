using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBite.DAL.Models;

namespace SmartBite.BAL.ReportManeger
{
    public class ReportService : IReportService
    {

        public string GenerateDiseaseReport(List<FoodItemModel> foodItems , List<NutrientLimitModel> nutrientLimits)
        {
            StringBuilder report = new StringBuilder();

            // Add a title/header for the report
            report.AppendLine("📊 Disease ANALYSIS REPORT 📊");
            report.AppendLine("═════════════════════════════");
            report.AppendLine();

            // Check each nutrient limit
            foreach (var limit in nutrientLimits)
            {
                report.AppendLine($"🔍 {limit.NutrientName}: {limit.LimitPerDay}{limit.NutrientUnit} daily limit");
                report.AppendLine("──────────────────────────");

                bool limitExceededInAnyFood = false;
                foreach (var food in foodItems)
                {
                    // Check if this food item contains the nutrient we're checking
                    var matchingNutrient = food.Nutrients?.FirstOrDefault(n =>
                        n.Name?.Equals(limit.NutrientName, StringComparison.OrdinalIgnoreCase) == true);

                    if (matchingNutrient != null && matchingNutrient.AmountPerServing > limit.LimitPerDay)
                    {
                        report.AppendLine($"⚠️ EXCEEDED in {food.Name}:");
                        report.AppendLine($"   {matchingNutrient.AmountPerServing}{matchingNutrient.AmountUnit}  (limit: {limit.LimitPerDay}{limit.NutrientUnit})");
                        limitExceededInAnyFood = true;
                    }
                }

                if (!limitExceededInAnyFood)
                {
                    report.AppendLine("✓ No food items exceed this limit");
                }
                report.AppendLine();
            }


            report.AppendLine("📋 SUMMARY 📋");
            report.AppendLine("═══════════════");

            int totalExceeded = 0;
            foreach (var food in foodItems)
            {
                var exceededNutrients = new List<string>();
                foreach (var limit in nutrientLimits)
                {
                    var matchingNutrient = food.Nutrients?.FirstOrDefault(n =>
                        n.Name?.Equals(limit.NutrientName, StringComparison.OrdinalIgnoreCase) == true);

                    if (matchingNutrient != null && matchingNutrient.AmountPerServing > limit.LimitPerDay)
                    {
                        exceededNutrients.Add($"{limit.NutrientName} ({matchingNutrient.AmountPerServing}{matchingNutrient.AmountUnit} out of limit {limit.LimitPerDay}{limit.NutrientUnit})");
                        totalExceeded++;
                    }
                }

                if (exceededNutrients.Count > 0)
                {
                    report.AppendLine($"🍽 {food.Name} exceeds limits for:");
                    foreach (var nutrient in exceededNutrients)
                    {
                        report.AppendLine($"   • {nutrient}");
                    }
                    report.AppendLine();
                }
            }

            if (totalExceeded == 0)
            {
                report.AppendLine("✅ No nutrient limits exceeded in any food items!");
            }
            else
            {
                report.AppendLine("💡 RECOMMENDATIONS 💡");
                report.AppendLine("─────────────────────");
                report.AppendLine("For your health and wellbeing:");
                report.AppendLine("• Consider reducing intake of these items");
                report.AppendLine("• Choose alternatives with lower levels");
                report.AppendLine("• Small changes make a big difference!");
            }

            return report.ToString();
        }

        public string GenerateAllergyReport(List<FoodItemModel> foodItems, List<AllergyItemModel> allergies)
        {
            StringBuilder report = new StringBuilder();
            // Track the total number of allergy issues found
            int totalAllergyIssues = 0;

            // First section: check each allergy against all foods
            report.AppendLine("🚨 ALLERGY CHECK REPORT 🚨");
            report.AppendLine("═════════════════════════");
            report.AppendLine();

            foreach (var allergy in allergies)
            {
                report.AppendLine($"🔎 Checking for {allergy.Name} allergies:");
                report.AppendLine("──────────────────────────");
                bool allergensFoundInAnyFood = false;

                foreach (var food in foodItems)
                {
                    // Check if food name matches any harmful item for this allergy
                    bool nameMatches = allergy.HarmfulItems!.Any(item =>
                        food.Name!.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0);

                    // Also check if any nutrient name matches harmful items
                    // This assumes some allergens might be listed in nutrients
                    bool nutrientMatches = food.Nutrients?.Any(nutrient =>
                        allergy.HarmfulItems!.Any(item =>
                            nutrient.Name!.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)) ?? false;

                    if (nameMatches || nutrientMatches)
                    {
                        report.AppendLine($"  ⚠️ WARNING: {food.Name} may contain allergens for {allergy.Name}");
                        allergensFoundInAnyFood = true;
                        totalAllergyIssues++;
                    }
                }

                if (!allergensFoundInAnyFood)
                {
                    report.AppendLine($"  ✅ No food items containing {allergy.Name} allergens found");
                }

                report.AppendLine(); // Add empty line between allergy reports
            }

            // Second section: organize by food item
            report.AppendLine("🍽 FOOD ITEM SUMMARY 🍽");
            report.AppendLine("═══════════════════════");
            report.AppendLine();

            bool anyFoodWithAllergies = false;
            foreach (var food in foodItems)
            {
                var triggeredAllergies = new List<string>();

                foreach (var allergy in allergies)
                {
                    // Check if food name matches any harmful item for this allergy
                    bool nameMatches = allergy.HarmfulItems!.Any(item =>
                        food.Name!.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0);

                    // Also check if any nutrient name matches harmful items
                    bool nutrientMatches = food.Nutrients?.Any(nutrient =>
                        allergy.HarmfulItems!.Any(item =>
                            nutrient.Name!.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)) ?? false;

                    if (nameMatches || nutrientMatches)
                    {
                        triggeredAllergies.Add(allergy.Name!);
                    }
                }

                if (triggeredAllergies.Count > 0)
                {
                    report.AppendLine($"🔴 {food.Name} may trigger allergies:");
                    foreach (var allergyName in triggeredAllergies)
                    {
                        report.AppendLine($"   • {allergyName}");
                    }
                    report.AppendLine();
                    anyFoodWithAllergies = true;
                }
            }

            if (!anyFoodWithAllergies)
            {
                report.AppendLine("✅ No foods with allergy concerns detected.");
                report.AppendLine();
            }

            // Final summary section
            report.AppendLine("📋 SUMMARY 📋");
            report.AppendLine("═════════════");

            if (totalAllergyIssues == 0)
            {
                report.AppendLine("✅ No allergy concerns detected in any food items.");
            }
            else
            {
                report.AppendLine("⚠️ IMPORTANT ALLERGY WARNING ⚠️");
                report.AppendLine("───────────────────────────────");
                report.AppendLine("• The food items listed above may contain ingredients that could trigger allergic reactions.");
                report.AppendLine("• If you have severe allergies, please verify ingredients directly with product manufacturers.");
                report.AppendLine("• Always consult with a healthcare professional regarding dietary restrictions related to allergies.");
            }

            return report.ToString();
        }

        public string GenerateAllergyandDiseaseReport(List<FoodItemModel> foodItems, List<AllergyItemModel> allergies, List<NutrientLimitModel> nutrientLimits)
        {
            StringBuilder report = new StringBuilder();

            // Main header
            report.AppendLine("🏥 COMPREHENSIVE HEALTH & ALLERGY ANALYSIS 🏥");
            report.AppendLine("════════════════════════════════════════════");
            report.AppendLine();

            // Track totals for final summary
            int totalAllergyIssues = 0;
            int totalNutrientExceeded = 0;

            // ============ ALLERGY ANALYSIS SECTION ============
            report.AppendLine("🚨 ALLERGY CHECK REPORT 🚨");
            report.AppendLine("═════════════════════════");
            report.AppendLine();

            foreach (var allergy in allergies)
            {
                report.AppendLine($"🔎 Checking for {allergy.Name} allergies:");
                report.AppendLine("──────────────────────────");
                bool allergensFoundInAnyFood = false;

                foreach (var food in foodItems)
                {
                    // Check if food name matches any harmful item for this allergy
                    bool nameMatches = allergy.HarmfulItems!.Any(item =>
                        food.Name!.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0);

                    // Also check if any nutrient name matches harmful items
                    bool nutrientMatches = food.Nutrients?.Any(nutrient =>
                        allergy.HarmfulItems!.Any(item =>
                            nutrient.Name!.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)) ?? false;

                    if (nameMatches || nutrientMatches)
                    {
                        report.AppendLine($"  ⚠️ WARNING: {food.Name} may contain allergens for {allergy.Name}");
                        allergensFoundInAnyFood = true;
                        totalAllergyIssues++;
                    }
                }

                if (!allergensFoundInAnyFood)
                {
                    report.AppendLine($"  ✅ No food items containing {allergy.Name} allergens found");
                }

                report.AppendLine(); // Add empty line between allergy reports
            }

            // ============ NUTRIENT LIMITS ANALYSIS SECTION ============
            report.AppendLine("📊 NUTRIENT LIMITS ANALYSIS 📊");
            report.AppendLine("═════════════════════════════");
            report.AppendLine();

            foreach (var limit in nutrientLimits)
            {
                report.AppendLine($"🔍 {limit.NutrientName}: {limit.LimitPerDay}{limit.NutrientUnit} daily limit");
                report.AppendLine("──────────────────────────");
                bool limitExceededInAnyFood = false;

                foreach (var food in foodItems)
                {
                    // Check if this food item contains the nutrient we're checking
                    var matchingNutrient = food.Nutrients?.FirstOrDefault(n =>
                        n.Name?.Equals(limit.NutrientName, StringComparison.OrdinalIgnoreCase) == true);

                    if (matchingNutrient != null && limit.LimitPerDay.HasValue && matchingNutrient.AmountPerServing > limit.LimitPerDay.Value)
                    {
                        report.AppendLine($"⚠️ EXCEEDED in {food.Name}:");
                        report.AppendLine($"   {matchingNutrient.AmountPerServing}{matchingNutrient.AmountUnit} (limit: {limit.LimitPerDay}{limit.NutrientUnit})");
                        limitExceededInAnyFood = true;
                        totalNutrientExceeded++;
                    }
                }

                if (!limitExceededInAnyFood)
                {
                    report.AppendLine("✓ No food items exceed this limit");
                }
                report.AppendLine();
            }

            // ============ FOOD ITEM COMPREHENSIVE SUMMARY ============
            report.AppendLine("🍽 COMPREHENSIVE FOOD ITEM ANALYSIS 🍽");
            report.AppendLine("════════════════════════════════════════");
            report.AppendLine();

            bool anyFoodWithIssues = false;
            foreach (var food in foodItems)
            {
                var triggeredAllergies = new List<string>();
                var exceededNutrients = new List<string>();

                // Check allergies for this food
                foreach (var allergy in allergies)
                {
                    bool nameMatches = allergy.HarmfulItems!.Any(item =>
                        food.Name!.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0);

                    bool nutrientMatches = food.Nutrients?.Any(nutrient => allergy.HarmfulItems!.Any(item =>
                        nutrient.Name!.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)) ?? false;

                    if (nameMatches || nutrientMatches)
                    {
                        triggeredAllergies.Add(allergy.Name!);
                    }
                }

                // Check nutrient limits for this food
                foreach (var limit in nutrientLimits)
                {
                    var matchingNutrient = food.Nutrients?.FirstOrDefault(n =>
                        n.Name?.Equals(limit.NutrientName, StringComparison.OrdinalIgnoreCase) == true);

                    if (matchingNutrient != null && limit.LimitPerDay.HasValue && matchingNutrient.AmountPerServing > limit.LimitPerDay.Value)
                    {
                        exceededNutrients.Add($"{limit.NutrientName} ({matchingNutrient.AmountPerServing}{matchingNutrient.AmountUnit} exceeds {limit.LimitPerDay}{limit.NutrientUnit})");
                    }
                }

                // Report if this food has any issues
                if (triggeredAllergies.Count > 0 || exceededNutrients.Count > 0)
                {
                    report.AppendLine($"🔴 {food.Name} has the following concerns:");

                    if (triggeredAllergies.Count > 0)
                    {
                        report.AppendLine("   🚨 ALLERGY TRIGGERS:");
                        foreach (var allergyName in triggeredAllergies)
                        {
                            report.AppendLine($"      • {allergyName}");
                        }
                    }

                    if (exceededNutrients.Count > 0)
                    {
                        report.AppendLine("   📊 NUTRIENT LIMIT EXCEEDED:");
                        foreach (var nutrient in exceededNutrients)
                        {
                            report.AppendLine($"      • {nutrient}");
                        }
                    }

                    report.AppendLine();
                    anyFoodWithIssues = true;
                }
            }

            if (!anyFoodWithIssues)
            {
                report.AppendLine("✅ No health concerns or allergy triggers detected in any food items.");
                report.AppendLine();
            }

            // ============ FINAL COMPREHENSIVE SUMMARY ============
            report.AppendLine("📋 COMPREHENSIVE HEALTH SUMMARY 📋");
            report.AppendLine("═══════════════════════════════════");
            report.AppendLine();

            if (totalAllergyIssues == 0 && totalNutrientExceeded == 0)
            {
                report.AppendLine("🎉 EXCELLENT NEWS! 🎉");
                report.AppendLine("✅ No allergy concerns detected");
                report.AppendLine("✅ No nutrient limits exceeded");
                report.AppendLine("✅ All food items appear safe for consumption");
            }
            else
            {
                report.AppendLine("⚠️ HEALTH ADVISORY ⚠️");
                report.AppendLine("────────────────────");

                if (totalAllergyIssues > 0)
                {
                    report.AppendLine($"🚨 {totalAllergyIssues} allergy concern(s) detected");
                }

                if (totalNutrientExceeded > 0)
                {
                    report.AppendLine($"📊 {totalNutrientExceeded} nutrient limit(s) exceeded");
                }

                report.AppendLine();
                report.AppendLine("💡 HEALTH RECOMMENDATIONS 💡");
                report.AppendLine("─────────────────────────────");

                if (totalAllergyIssues > 0)
                {
                    report.AppendLine("• Consider reducing intake of items that affect your allergies");
                    report.AppendLine("• Look for healthier alternatives");
                    report.AppendLine("• Moderation is key for long-term health benefits");
                }

                if (totalNutrientExceeded > 0)
                {
                    report.AppendLine("• Consider reducing intake of items that exceed nutrient limits");
                    report.AppendLine("• Look for healthier alternatives with lower nutrient levels");
                    report.AppendLine("• Moderation is key for long-term health benefits");
                }

                report.AppendLine("• Small dietary changes can make a significant health impact!");
            }

            return report.ToString();
        }

        public string GenerateCaloriesReport(List<FoodItemModel> foodItems, decimal remainingDailyCalories)
        {
            StringBuilder report = new StringBuilder();

            // Calculate total energy from all food items
            decimal totalEnergyFromFoods = 0;
            foreach (var food in foodItems)
            {
                totalEnergyFromFoods += food.EnergyPerServing;
            }

            // Generate report
            report.AppendLine("🔥 CALORIE CHECK REPORT 🔥");
            report.AppendLine("═════════════════════════");
            report.AppendLine();

            report.AppendLine("📊 FOOD ITEMS ENERGY:");
            report.AppendLine("─────────────────────");
            foreach (var food in foodItems)
            {
                report.AppendLine($"• {food.Name}: {food.EnergyPerServing} calories");
            }
            report.AppendLine();

            report.AppendLine($"📈 Total Energy from Foods: {totalEnergyFromFoods} calories");
            report.AppendLine($"📉 Remaining Daily Calories: {remainingDailyCalories} calories");
            report.AppendLine();

            // Check if exceeds remaining calories
            if (totalEnergyFromFoods > remainingDailyCalories)
            {
                decimal excess = totalEnergyFromFoods - remainingDailyCalories;
                report.AppendLine("🚨 CALORIE LIMIT EXCEEDED! 🚨");
                report.AppendLine("═══════════════════════════");
                report.AppendLine($"⚠️ You have exceeded your remaining daily calories by {excess} calories!");
                report.AppendLine("💡 Consider reducing portions or choosing lower-calorie alternatives.");
            }
            else
            {
                decimal remaining = remainingDailyCalories - totalEnergyFromFoods;
                report.AppendLine("✅ WITHIN CALORIE LIMIT ✅");
                report.AppendLine("═════════════════════════");
                report.AppendLine($"👍 You are within your daily calorie limit!");
                report.AppendLine($"📋 After consuming these foods, you'll have {remaining} calories remaining.");
            }

            return report.ToString();
        }

        public string GenerateProductAllergyReport(List<string> ingredients, List<AllergyIngModel> allergies)
        {
            StringBuilder report = new StringBuilder();
            // Track the total number of allergy issues found
            int totalAllergyIssues = 0;

            // First section: check each allergy against all ingredients
            report.AppendLine("🚨 PRODUCT ALLERGY CHECK REPORT 🚨");
            report.AppendLine("═════════════════════════════════");
            report.AppendLine();

            foreach (var allergy in allergies)
            {
                report.AppendLine($"🔎 Checking for {allergy.Name} allergies:");
                report.AppendLine("──────────────────────────");
                bool allergensFoundInAnyIngredient = false;

                foreach (var ingredient in ingredients)
                {
                    // Check if ingredient matches any harmful ingredient for this allergy
                    bool ingredientMatches = allergy.HarmfulIngredients!.Any(harmfulItem =>
                        ingredient.IndexOf(harmfulItem, StringComparison.OrdinalIgnoreCase) >= 0);

                    if (ingredientMatches)
                    {
                        report.AppendLine($"  ⚠️ WARNING: '{ingredient}' may contain allergens for {allergy.Name}");
                        allergensFoundInAnyIngredient = true;
                        totalAllergyIssues++;
                    }
                }

                if (!allergensFoundInAnyIngredient)
                {
                    report.AppendLine($"  ✅ No ingredients containing {allergy.Name} allergens found");
                }

                report.AppendLine(); // Add empty line between allergy reports
            }

            // Second section: organize by ingredient
            report.AppendLine("🧪 INGREDIENT SUMMARY 🧪");
            report.AppendLine("═══════════════════════");
            report.AppendLine();

            bool anyIngredientWithAllergies = false;
            foreach (var ingredient in ingredients)
            {
                var triggeredAllergies = new List<string>();

                foreach (var allergy in allergies)
                {
                    // Check if ingredient matches any harmful ingredient for this allergy
                    bool ingredientMatches = allergy.HarmfulIngredients!.Any(harmfulItem =>
                        ingredient.IndexOf(harmfulItem, StringComparison.OrdinalIgnoreCase) >= 0);

                    if (ingredientMatches)
                    {
                        triggeredAllergies.Add(allergy.Name!);
                    }
                }

                if (triggeredAllergies.Count > 0)
                {
                    report.AppendLine($"🔴 '{ingredient}' may trigger allergies:");
                    foreach (var allergyName in triggeredAllergies)
                    {
                        report.AppendLine($"   • {allergyName}");
                    }
                    report.AppendLine();
                    anyIngredientWithAllergies = true;
                }
            }

            if (!anyIngredientWithAllergies)
            {
                report.AppendLine("✅ No ingredients with allergy concerns detected.");
                report.AppendLine();
            }

            // Final summary section
            report.AppendLine("📋 SUMMARY 📋");
            report.AppendLine("═════════════");

            if (totalAllergyIssues == 0)
            {
                report.AppendLine("✅ No allergy concerns detected in any product ingredients.");
            }
            else
            {
                report.AppendLine("⚠️ IMPORTANT ALLERGY WARNING ⚠️");
                report.AppendLine("───────────────────────────────");
                report.AppendLine("• The ingredients listed above may contain components that could trigger allergic reactions.");
                report.AppendLine("• If you have severe allergies, please verify ingredients directly with product manufacturers.");
                report.AppendLine("• Always consult with a healthcare professional regarding product usage and allergic reactions.");
                report.AppendLine("• Consider performing a patch test before using products with concerning ingredients.");
            }

            return report.ToString();
        }



    }
}
