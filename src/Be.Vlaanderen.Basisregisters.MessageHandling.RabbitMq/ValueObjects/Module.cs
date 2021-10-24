using System;

namespace Be.Vlaanderen.Basisregisters.MessageHandling.RabbitMq
{
    public readonly struct Module
    {
        public static Module Address = new("address-registry");
        public static Module StreetName = new("streetname-registry");
        public static Module Building = new("building-registry");
        public static Module Municipality = new("municipality-registry");
        public static Module Parcel = new("parcel-registry");
        public static Module Postal = new("postal-registry");

        public string Value { get; }

        private Module(string value) => Value = value;

        public static Module? Parse(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            if (name != Address.Value &&
                name != StreetName.Value &&
                name != Building.Value &&
                name != Municipality.Value &&
                name != Parcel.Value &&
                name != Postal.Value)
                throw new NotImplementedException($"Cannot parse {name} to {nameof(Module)}");

            return new Module(name);
        }
        public override string ToString() => Value;
        public static implicit operator string(Module name) => name.Value;
    }
}
