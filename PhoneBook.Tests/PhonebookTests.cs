using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace PhoneBook.Tests
{
  public class PhonebookTests
  {
    private Phonebook phonebook;
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
      this.phonebook = new Phonebook();
    }

    [TearDown]
    public void TearDown()
    {
      this.phonebook.ClearPhonebookList();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
      this.phonebook = null;
    }

    [TestCase("EAB77320-EC98-453A-87EC-454C34DBDA1C", "Test")]
    [TestCase("E683FD10-9F3E-47CA-A7A1-47749512B94C", "Test2")]
    [TestCase("E683FD10-9F3E-47CA-A7A1-47749512B94C", "")]
    public void AddSubscriber_NewSubscriber_AddedSuccesfully(string id, string name)
    {
      Guid subscriberId = Guid.Parse(id);
      var expectedSubscriber = new Subscriber(subscriberId, name, new List<PhoneNumber>());
      this.phonebook.AddSubscriber(expectedSubscriber);

      Assert.That(this.phonebook.GetSubscriber(subscriberId), Is.EqualTo(expectedSubscriber));
    }

    [Test]
    public void AddSubscriber_NewSubscriberWithEmptyId_AddedSuccesfully()
    {
      Guid subscriberId = Guid.Empty;
      string subscriberName = "Test";
      var expectedSubscriber = new Subscriber(subscriberId, subscriberName, new List<PhoneNumber>());
      this.phonebook.AddSubscriber(expectedSubscriber);

      Assert.That(this.phonebook.GetSubscriber(subscriberId), Is.EqualTo(expectedSubscriber));
    }

    [Test]
    public void AddSubscriber_ExistingSubscriber_ThrowsException()
    {
      Guid subscriberId = Guid.NewGuid();
      var subscriber = new Subscriber(subscriberId, "Test", new List<PhoneNumber>());
      this.phonebook.AddSubscriber(subscriber);

      var exception = Assert.Throws<InvalidOperationException>(() => this.phonebook.AddSubscriber(subscriber));
      Assert.That(exception.Message, Is.EqualTo("Exist"));
    }


    [Test]
    public void DeleteSubscriber_ExistingSubscriber_RemovesSubscriberSuccessfully()
    {
      Guid subscriberId = Guid.NewGuid();
      var subscriber = new Subscriber(subscriberId, "Test", new List<PhoneNumber>());
      this.phonebook.AddSubscriber(subscriber);

      this.phonebook.DeleteSubscriber(subscriber);

      var exception = Assert.Throws<InvalidOperationException>(() => this.phonebook.GetSubscriber(subscriberId));
      Assert.That(exception.Message, Is.EqualTo("Exception"));
    }

    [Test]
    public void GetAll_ReturnsAllSubscribers()
    {
      var subscribers = new List<Subscriber>
      {
        new Subscriber(Guid.NewGuid(), "Test1", new List<PhoneNumber>()),
        new Subscriber(Guid.NewGuid(), "Test2", new List<PhoneNumber>())
      };

      foreach (var sub in subscribers)
      {
        this.phonebook.AddSubscriber(sub);
      }

      var allSubscribers = this.phonebook.GetAll();

      Assert.That(allSubscribers, Is.EquivalentTo(subscribers));
    }

    [Test]
    public void RenameSubscriber_ExistingSubscriber_NameUpdatedSuccessfully()
    {
      Guid subscriberId = Guid.NewGuid();
      var subscriber = new Subscriber(subscriberId, "OldName", new List<PhoneNumber>());
      this.phonebook.AddSubscriber(subscriber);

      this.phonebook.RenameSubscriber(subscriber, "NewName");

      var updatedSubscriber = this.phonebook.GetSubscriber(subscriberId);
      Assert.That(updatedSubscriber.Name, Is.EqualTo("NewName"));
    }

    [Test]
    public void AddNumberToSubscriber_ExistingSubscriber_AddsNumberSuccessfully()
    {
      Guid subscriberId = Guid.NewGuid();
      var subscriber = new Subscriber(subscriberId, "Test", new List<PhoneNumber>());
      this.phonebook.AddSubscriber(subscriber);

      var newNumber = new PhoneNumber("123-456-7890", PhoneNumberType.Work);
      this.phonebook.AddNumberToSubscriber(subscriber, newNumber);

      var updatedSubscriber = this.phonebook.GetSubscriber(subscriberId);
      Assert.That(updatedSubscriber.PhoneNumbers, Contains.Item(newNumber));
    }
  }
}
