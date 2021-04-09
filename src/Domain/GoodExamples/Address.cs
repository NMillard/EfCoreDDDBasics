namespace Domain.GoodExamples {
    public class Address {
        public Address(string street, string houseNumber, string city, string zipcode) {
            Street = street;
            HouseNumber = houseNumber;
            City = city;
            Zipcode = zipcode;
        }
        
        public string Street { get; }
        public string HouseNumber { get; }
        public string City { get; }
        public string Zipcode { get; }
    }
}