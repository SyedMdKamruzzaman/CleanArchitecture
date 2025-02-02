﻿using System;

namespace EmployeeManagement.Domain.Primitives;

public abstract class Entity : IEquatable<Entity>
{
    public Guid Id { get; protected init; }

    public static bool operator ==(Entity first, Entity second)
    {
        if (first is null && second is null)
        {
            return true;
        }

        if (first is null || second is null)
        {
            return false;
        }

        return first.Equals(second);
    }

    public static bool operator !=(Entity first, Entity second)
    {
        return !(first == second);
    }

    public bool Equals(Entity other)
    {
        if (other is null)
        {
            return false;
        }

        if (other.GetType() != GetType())
        {
            return false;
        }

        return other.Id == Id;
    }

    public override bool Equals(object obj)
    {
        if (obj is null)
        {
            return false;
        }

        // Chekc if the two have same type.
        if (obj.GetType() != GetType())
        {
            return false;
        }

        // Check If the obj if of type Entity.
        if (obj is not Entity entity)
        {
            return false;
        }

        return entity.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
