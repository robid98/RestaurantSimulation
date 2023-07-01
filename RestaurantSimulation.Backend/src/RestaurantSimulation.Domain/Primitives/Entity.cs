﻿namespace RestaurantSimulation.Domain.Primitives
{
    public abstract class Entity : IEquatable<Entity>
    {
        protected Entity(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private init; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }


            if (obj.GetType() != GetType())
            {
                return false;
            }

            if (obj is not Entity entity)
            {
                return false;
            }

            return entity.Id == Id;
        }

        public bool Equals(Entity other)
        {
            if(other == null)
            {
                return false;
            }

            if (other.GetType() != GetType())
                return false;

            return other.Id == Id;
        }

        public static bool operator==(Entity first, Entity second)
        {
            return first is not null && second is not null && first.Equals(second);
        }

        public static bool operator!=(Entity first, Entity second)
        {
            return !(first == second);
        }


        public override int GetHashCode()
        {
            return Id.GetHashCode() * 41;
        }

    }
}
