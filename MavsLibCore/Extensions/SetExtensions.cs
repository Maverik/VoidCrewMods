using System.Linq.Expressions;
using BepInEx.Logging;
using Gameplay.Utilities;

namespace MavsLibCore;

public static class SetExtensions
{
    public static T Set<T>(ref T field, T value, ManualLogSource? pluginLogger = default, [CallerArgumentExpression(nameof(field))] string fieldName = default!)
    {
        if (field?.Equals(value) ?? false) return field;

        var logger = pluginLogger ?? MavLogger.Default;

        logger.LogDebug($"{fieldName} was {field}");

        field = value;

        logger.LogInfo($"{fieldName} is {field}");

        return field;
    }

    [SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
    public static TInstance Set<TInstance, TValue>(this TInstance? instance, MemberInfo info, Func<TValue, TValue> valueFunc, ManualLogSource? pluginLogger = default, [CallerArgumentExpression(nameof(info))] string infoName = default!)
    {
        if (instance is null) throw new ArgumentNullException(nameof(instance));
        if (info is null) throw new ArgumentNullException(nameof(info));

        var logger = pluginLogger ?? MavLogger.Default;

        var typeName = typeof(TInstance).Name;

        var fi = info as FieldInfo;
        var pi = info as PropertyInfo;

        if (fi is null && pi is null) throw new InvalidOperationException("No supported member type found for Set");

        var isFieldInfo = fi is not null;

        var fiValue = (TValue)(isFieldInfo ? fi!.GetValue(instance) : pi!.GetValue(instance));
        var fiValueHasAmount = (isFieldInfo ? fi!.FieldType : pi!.PropertyType).GetField("Amount");
        var fiValueHasValue = (isFieldInfo ? fi!.FieldType : pi!.PropertyType).GetProperty("Value");

        var effectiveDisplayValue = GetEffectiveDisplayValue(fiValue, fiValueHasValue, fiValueHasAmount);

        var transformedValue = valueFunc(fiValue);

        if (string.Equals(effectiveDisplayValue?.ToString(), GetEffectiveDisplayValue(transformedValue, fiValueHasValue, fiValueHasAmount)?.ToString(), StringComparison.Ordinal)) return instance;

        logger.LogDebug($"{typeName}.{infoName} was {effectiveDisplayValue}");

        if (isFieldInfo)
            fi?.SetValue(instance, transformedValue);
        else
            pi?.SetValue(instance, transformedValue);

        fiValue = (TValue)(isFieldInfo ? fi!.GetValue(instance) : pi!.GetValue(instance));

        effectiveDisplayValue = GetEffectiveDisplayValue(fiValue, fiValueHasValue, fiValueHasAmount);

        logger.LogInfo($"{typeName}.{infoName} is {effectiveDisplayValue}");

        return instance;
    }

    static object? GetEffectiveDisplayValue<T>(T? fiValue, PropertyInfo? fiValueHasValue, FieldInfo? fiValueHasAmount)
    {
        object? bufferValue = fiValue;

        if (fiValue is not null && fiValueHasAmount is not null)
            bufferValue = fiValueHasAmount.GetValue(fiValue);
        else if (fiValue is not null && fiValueHasValue is not null)
            bufferValue = fiValueHasValue.GetValue(fiValue);

        return bufferValue;
    }

    public static TInstance? Set<TInstance, TValue>(this TInstance? instance, Expression<Func<TInstance, TValue>> selector, Func<TValue, TValue> valueFunc, ManualLogSource? pluginLogger = default, [CallerArgumentExpression(nameof(instance))] string infoName = default!)
    {
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        if (infoName is "__instance" or "__result") infoName = typeof(TInstance).Name;

        var logger = pluginLogger ?? MavLogger.Default;

        if (instance is null)
        {
            logger.LogDebug($"{infoName} was null");

            return instance;
        }

        if (selector.Body is MemberExpression member)
            instance.Set(member.Member, valueFunc, logger, $"{infoName}:{member.Member.Name}");
        else
            logger.LogError("Unsupported expression given for SetValue. No configuration value set.");

        return instance;
    }

    public static TInstance? Set<TInstance, TValue>(this TInstance? instance, TValue value, ManualLogSource? pluginLogger = default, [CallerArgumentExpression(nameof(instance))] string fieldName = default!) where TInstance : PrimitiveModifier<TValue>
    {
        var logger = pluginLogger ?? MavLogger.Default;

        if (fieldName is "__instance" or "__result") fieldName = typeof(TInstance).Name;

        if (instance is null)
        {
            logger.LogDebug($"{fieldName} was null");

            return instance;
        }

        logger.LogDebug($"{fieldName} was {instance.Amount}");

        instance.Amount = value;
        instance.AffectedMod?.RecalculateValue();

        logger.LogInfo($"{fieldName} is {instance.Amount}");

        return instance;
    }

    public static TInstance? Set<TInstance, TValue, TValueModifier>(this TInstance? instance, TValue value, ManualLogSource? pluginLogger = default, [CallerArgumentExpression(nameof(instance))] string fieldName = default!)
        where TInstance : ModifiablePrimitive<TValue, TValueModifier>
        where TValueModifier : PrimitiveModifier<TValue>
    {
        var logger = pluginLogger ?? MavLogger.Default;

        if (instance is null)
        {
            logger.LogDebug($"{fieldName} was null");

            return instance;
        }

        if (fieldName is "__instance" or "__result") fieldName = typeof(TInstance).Name;

        logger.LogDebug($"{fieldName} was base: {instance.BaseValue}, value: {instance.Value}");

        instance.SetBaseValue(value);

        logger.LogInfo($"{fieldName} is base: {instance.BaseValue}, value: {instance.Value}");

        return instance;
    }
}