namespace Hive.Test;

public class Tests
{
	[SetUp]
	public void Setup()
	{
	}

	[Test]
	public void Test1()
	{
		Assert.That(2, Is.EqualTo(2));
	}
}