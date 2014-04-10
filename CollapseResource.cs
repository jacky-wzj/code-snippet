    /*
        Get Resource with collapsed
    */

    // base class , that define self collapse or child collapse
    [Serializable]
    [DataContract]
    public class CollapsedBase
    {
        public CollapsedBase()
        {
            SelfCollapsed = false;
            ChildrenCollapsed = false;
        }

        /// <summary>
        /// determine whether the object collaspe
        /// </summary>
        [DataMember]
        public bool SelfCollapsed { get; set; }

        /// <summary>
        /// determine whether the object collaspe children
        /// </summary>
        [DataMember]
        public bool ChildrenCollapsed { get; set; }
    }


    // final class, that define which part collapse (the collapsed resoure must using same name wiht origin porperty)
    [Serializable]
    [DataContract]
    public class VacationProductCollapsed : CollapsedBase
    {
        public VacationProductCollapsed()
        {
            VProductArrivedRelInfos = new VProductArrivedRelInfosCollapsed();
            VProductDepartureRelInfos = new VProductDepartureRelInfosCollapsed();
            ProductBasicInfos = new ProductBasicInfosCollapsed();
        }
        [DataMember]
        public VProductArrivedRelInfosCollapsed VProductArrivedRelInfos { get; set; }
        [DataMember]
        public VProductDepartureRelInfosCollapsed VProductDepartureRelInfos { get; set; }
        [DataMember]
        public ProductBasicInfosCollapsed ProductBasicInfos { get; set; }
    }

    [Serializable]
    [DataContract]
    public class VProductArrivedRelInfosCollapsed : CollapsedBase { }

    [Serializable]
    [DataContract]
    public class VProductDepartureRelInfosCollapsed : CollapsedBase { }

    [Serializable]
    [DataContract]
    public class ProductBasicInfosCollapsed : CollapsedBase
    {
        public ProductBasicInfosCollapsed()
        {
            ProductTourDailyInfos = new ProductTourDailyInfosCollapsed();
        }
        [DataMember]
        public ProductTourDailyInfosCollapsed ProductTourDailyInfos { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ProductTourDailyInfosCollapsed : CollapsedBase { }

    //collapse help 
    public static class CollapseResourceHelper
    {
        private const string SelfCollapsed = "SelfCollapsed";
        private const string ChildrenCollapsed = "ChildrenCollapsed";

        /// <summary>
        /// collpase the TResourceType.
        /// </summary>
        /// <typeparam name="TResourceType"></typeparam>
        /// <typeparam name="TCollapseType"></typeparam>
        /// <param name="resource"></param>
        /// <param name="collapse"></param>
        public static void Collapse<TResourceType, TCollapseType>(ref TResourceType resource, TCollapseType collapse)
            where TCollapseType : CollapsedBase, new()
            where TResourceType : class , new()
        {
            try
            {
                if (resource == null) return ;
                if (collapse == null) return;
                
                if (collapse.SelfCollapsed)
                {
                    resource = default(TResourceType);
                }
                else if (collapse.ChildrenCollapsed)
                {
                    var collapsedProperties = collapse.GetType().GetProperties();
                    foreach (var collapsedProperty in collapsedProperties)
                    {
                        if (collapsedProperty == null) continue;
                        if (collapsedProperty.Name.Equals(SelfCollapsed)) continue;
                        if (collapsedProperty.Name.Equals(ChildrenCollapsed)) continue;

                        var value = GetValue(collapsedProperty, collapse);
                        if (value != null && value is CollapsedBase)
                        {
                            var targetProperty = GetProperty(resource.GetType(), collapsedProperty.Name);
                            if (targetProperty != null)
                            {
                                var targetValue = GetValue(targetProperty, resource);
                                if (targetValue == null) continue;
                                var collapsedValue = value as CollapsedBase;
                                if (collapsedValue.SelfCollapsed)
                                {
                                    SetValue(targetProperty, resource, null);
                                }
                                else if (collapsedValue.ChildrenCollapsed)
                                {   
                                    if (targetValue is IDictionary)
                                    {
                                        //don't support Dictronary now.
                                    }
                                    else if(targetValue is IEnumerable)
                                    {
                                        var list = targetValue as IEnumerable;
                                        foreach (var l in list)
                                        {
                                            var temp = l;
                                            Collapse(ref temp, collapsedValue);
                                        }
                                    }
                                    else
                                    {
                                        Collapse(ref targetValue, collapsedValue);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //deal with unhandle exception
            }
            return;
        }
        
        public static void SetValue(PropertyInfo p, object obj, object value)
        {
            try
            {
                p.SetValue(obj, value, null);
            }
            catch (Exception e) { }
        }

        private static PropertyInfo GetProperty(Type t, string name)
        {
            PropertyInfo result = null;
            try
            {
                result = t.GetProperty(name);
            }
            catch (Exception e)
            {
                //deal with Exception
            }

            return result;
        }

        private static object GetValue(PropertyInfo p, object obj)
        {
            object result = null;
            try
            {
                result = p.GetValue(obj, null);
            }
            catch (System.ArgumentException ae)
            {
                //deal with ArgumentException
            }
            catch (System.Reflection.TargetException at)
            {
                //deal with TargetException
            }
            catch (System.Reflection.TargetParameterCountException de)
            {
                //deal with TargetParameterCountException
            }
            catch (System.MethodAccessException e)
            {
                //deal with MethodAccessException
            }
            catch (System.Reflection.TargetInvocationException e)
            {
                //deal with TargetInvocationException
            }
            return result;
        }
    }

    //use the collapse
    CollapseResourceHelper.Collapse(ref item, resourceCollapsed);