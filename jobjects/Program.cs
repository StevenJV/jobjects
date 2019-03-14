using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace jobjects
{
    class Program
    {

        private static readonly string _getResponseText = @"{
  ""statusCode"": 200,
  ""data"": {
    ""caseSections"": [
      {
        ""name"": ""Case"",
        ""isActive"": true,
        ""hierarchy"": 10,
        ""customFieldItems"": [
          {
            ""customFieldId"": 97,
            ""dataTypeId"": 5,
            ""dataType"": ""Text"",
            ""customFieldName"": ""TextField"",
            ""insightsFieldName"": ""TestTextField"",
            ""hierarchy"": 5,
            ""isActive"": true,
            ""rowVersion"": ""AAAAAAAAZjU="",
            ""isInsightsEligible"": true
          }
        ]
      },
      {
        ""name"": ""Location"",
        ""isActive"": true,
        ""hierarchy"": 20,
        ""customFieldItems"": [
          {
            ""customFieldId"": 999,
            ""dataTypeId"": 5,
            ""dataType"": ""Text"",
            ""customFieldName"": ""FieldName999"",
            ""insightsFieldName"": ""InsightsFieldName999"",
            ""hierarchy"": 5,
            ""isActive"": true,
            ""rowVersion"": ""AAAAAAAAZjc="",
            ""isInsightsEligible"": true
          },
          {
            ""customFieldId"": -1,
            ""dataTypeId"": 5,
            ""dataType"": ""Text"",
            ""customFieldName"": ""LocationCustomField1"",
            ""insightsFieldName"": ""Location1"",
            ""hierarchy"": 5,
            ""isActive"": true,
            ""rowVersion"": ""AAAAAAAAZjY="",
            ""isInsightsEligible"": true
          }
        ]
      }
    ],
    ""customerId"": 42,
    ""customerKey"": ""trial12""
  },
  ""errors"": []
}";


        private static void Main(string[] args)
        {
            string _testSectionName = "Location";
            string _testInsightsFieldName = "InsightsFieldName999";

            var dashObj = JObject.Parse(_getResponseText);

            Array sectionNames = GetArrayOfCaseSections(dashObj);
            Console.WriteLine($"there were {sectionNames.Length} sections found.");

            int foundSection = Array.IndexOf(sectionNames, _testSectionName);
            if (foundSection >= 0) Console.WriteLine($"section `{_testSectionName}` found, in caseSection [{foundSection}]"); //Assert.True(intTestSectionFound, $"{_testSectionName} section not found");

            var itemsInSection = dashObj["data"]["caseSections"][foundSection]["customFieldItems"]
                .Values<string>("insightsFieldName");
            var chosenItem = itemsInSection.SingleOrDefault(iis => iis.Contains(_testInsightsFieldName));

            if (String.Equals(chosenItem, _testInsightsFieldName)) Console.WriteLine($"insightsFieldName `{_testInsightsFieldName}` found"); //Assert.True(insightsFieldNameFound, $"{_testInsightsFieldName} not found");

            Console.ReadKey();
        }

        private static Array GetArrayOfCaseSections(JObject dashObj)
        {
            var tmp = dashObj["data"]["caseSections"].Values<string>("name");
            Array sectionNames = tmp.ToArray();
            return sectionNames;
        }
    }
}