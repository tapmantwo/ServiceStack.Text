using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ServiceStack.Text;

namespace ServiceStack.ServiceModel.Tests
{
	[TestFixture]
	public class StringConverterUtilsTests
	{
		public class StringEnumerable : IEnumerable<string>
		{
			public List<string> Items = new[] { "a", "b", "c" }.ToList();

			public IEnumerator<string> GetEnumerator()
			{
				return Items.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public static StringEnumerable ParseJsv(string value)
			{
				return new StringEnumerable {
					Items = value.To<List<string>>()
				};
			}

		}
#if !IOS
		[Test]
		public void Create_super_list_type_of_int_from_string()
		{
			var textValue = "1,2,3";
			var convertedValue = textValue.Split(',').ToList().ConvertAll(x => Convert.ToInt32(x));
			var result = TypeSerializer.DeserializeFromString<ArrayOfIntId>(textValue);
			Assert.That(result, Is.EquivalentTo(convertedValue));
		}
#endif

		[Test]
		public void Create_guid_from_string()
		{
			var textValue = "40DFA5A2-8054-4b3e-B7F5-06E61FF387EF";
			var convertedValue = new Guid(textValue);
			var result = TypeSerializer.DeserializeFromString<Guid>(textValue);
			Assert.That(result, Is.EqualTo(convertedValue));
		}

		[Test]
		public void Create_int_from_string()
		{
			var textValue = "99";
			var convertedValue = int.Parse(textValue);
			var result = TypeSerializer.DeserializeFromString<int>(textValue);
			Assert.That(result, Is.EqualTo(convertedValue));
		}

		[Test]
		public void Create_bool_from_string()
		{
			var textValue = "True";
			var convertedValue = bool.Parse(textValue);
			var result = TypeSerializer.DeserializeFromString<bool>(textValue);
			Assert.That(result, Is.EqualTo(convertedValue));
		}

		[Test]
		public void Create_string_array_from_string()
		{
			var convertedValue = new[] { "Hello", "World" };
			var textValue = string.Join(",", convertedValue);
			var result = TypeSerializer.DeserializeFromString<string[]>(textValue);
			Assert.That(result, Is.EqualTo(convertedValue));
		}

		[Test]
		public void Create_from_StringEnumerable()
		{
			var value = StringEnumerable.ParseJsv("d,e,f");
			var convertedValue = TypeSerializer.SerializeToString(value);
			var result = TypeSerializer.DeserializeFromString<StringEnumerable>(convertedValue);
			Assert.That(result, Is.EquivalentTo(value.Items));
		}

        [Test]
	    public void Create_from_GuidEnumerable()
        {
            IEnumerable<Guid> list = new[] { new Guid("a7bc4846-ec70-4d8a-bda3-22a8d35d0d4e"), new Guid("ee8fc7a5-a0e2-48fa-b315-93bf8c6ed937") };
            var convertedValue = TypeSerializer.SerializeToString(list);
            Assert.AreEqual("[a7bc4846-ec70-4d8a-bda3-22a8d35d0d4e,ee8fc7a5-a0e2-48fa-b315-93bf8c6ed937]", convertedValue);
        }
	}
}