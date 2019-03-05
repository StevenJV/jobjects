using System;
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

            Console.WriteLine("Hello World!");
            Console.WriteLine("---");


            var dashObj = JObject.Parse(_getResponseText);

            var sectionNames =
                from p in dashObj["data"]["caseSections"]
                select (string)p["name"];
            var intTestSectionFound = false;
            int numberOfSections = 0;
            int foundSection = 0;
            foreach (var item in sectionNames)
            {
                numberOfSections++;
                if (string.Equals(item, _testSectionName))
                {
                    intTestSectionFound = true;
                    foundSection = numberOfSections - 1;
                }
            }
            if (intTestSectionFound) Console.WriteLine($"{_testSectionName}  found");//Assert.True(intTestSectionFound, $"{_testSectionName} section not found");

            Console.WriteLine("---");

            var a = from p in dashObj["data"]["caseSections"][foundSection]["customFieldItems"]
                    select (string)p["insightsFieldName"];
            var insightsFieldNameFound = false;
            foreach (var b in a)
            {
                if (b.Contains(_testInsightsFieldName))
                    insightsFieldNameFound = true;
            }
            //Assert.True(insightsFieldNameFound, $"{_testInsightsFieldName} not found");
            if (insightsFieldNameFound) Console.WriteLine($"{_testInsightsFieldName}  found");// Assert.isTrue(insightsFieldNameFound);

            Console.WriteLine("---");
            Console.ReadKey();
        }
    }
}