using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optional<T>
{
    public T Value;
    public bool HasValue { get { return Value != null; } }
    public T GetValueOrDefault()
    {
        if (HasValue)
            return Value;
        try
        {
            return default(T);
        } catch
        {
            throw new System.InvalidOperationException("Cannot find default of: " + typeof(T).FullName);
        }
    }

    public Optional(T item)
    {
        Value = item;
    }
    public Optional()
    {
        Value = default(T);
    }

    public override string ToString()
    {
        if(this.HasValue)
            return "Type " + typeof(T).FullName + "  - Value: " + Value.ToString();
        return "Type " + typeof(T).FullName + "  - Value: <null>";
    }

    public override bool Equals(object obj)
    {
        if(obj is Optional<T>)
        {
            var casted = (Optional<T>)obj;
            if (this.HasValue)
                return this.Value.Equals(casted.Value);
            else if(casted.HasValue)
            {
                casted.Value.Equals(this.Value);
            }
        }
        return false;
    }

    public static bool operator ==(Optional<T> left, Optional<T> right)
    {
        if(left.HasValue && right.HasValue)
        {
            return left.Equals(right);
        }
        return false;
    }
    public static bool operator !=(Optional<T> left, Optional<T> right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        if(HasValue)
            return Value.GetHashCode();
        return base.GetHashCode();
    }
}
