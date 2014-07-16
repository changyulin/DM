﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18444
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DM.Repository
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Shop")]
	public partial class ShopDBDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region 可扩展性方法定义
    partial void OnCreated();
    partial void InsertBrands(Brands instance);
    partial void UpdateBrands(Brands instance);
    partial void DeleteBrands(Brands instance);
    partial void InsertCategory(Category instance);
    partial void UpdateCategory(Category instance);
    partial void DeleteCategory(Category instance);
    partial void InsertProduct(Product instance);
    partial void UpdateProduct(Product instance);
    partial void DeleteProduct(Product instance);
    #endregion
		
		public ShopDBDataContext() : 
				base(global::DM.Repository.Properties.Settings.Default.ShopConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public ShopDBDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ShopDBDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ShopDBDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ShopDBDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Brands> Brands
		{
			get
			{
				return this.GetTable<Brands>();
			}
		}
		
		public System.Data.Linq.Table<Category> Category
		{
			get
			{
				return this.GetTable<Category>();
			}
		}
		
		public System.Data.Linq.Table<Product> Product
		{
			get
			{
				return this.GetTable<Product>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Brands")]
	public partial class Brands : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _BrandID;
		
		private string _BrandName;
		
		private EntitySet<Product> _Product;
		
    #region 可扩展性方法定义
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnBrandIDChanging(string value);
    partial void OnBrandIDChanged();
    partial void OnBrandNameChanging(string value);
    partial void OnBrandNameChanged();
    #endregion
		
		public Brands()
		{
			this._Product = new EntitySet<Product>(new Action<Product>(this.attach_Product), new Action<Product>(this.detach_Product));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BrandID", DbType="VarChar(50) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string BrandID
		{
			get
			{
				return this._BrandID;
			}
			set
			{
				if ((this._BrandID != value))
				{
					this.OnBrandIDChanging(value);
					this.SendPropertyChanging();
					this._BrandID = value;
					this.SendPropertyChanged("BrandID");
					this.OnBrandIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BrandName", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string BrandName
		{
			get
			{
				return this._BrandName;
			}
			set
			{
				if ((this._BrandName != value))
				{
					this.OnBrandNameChanging(value);
					this.SendPropertyChanging();
					this._BrandName = value;
					this.SendPropertyChanged("BrandName");
					this.OnBrandNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Brands_Product", Storage="_Product", ThisKey="BrandID", OtherKey="BrandID")]
		public EntitySet<Product> Product
		{
			get
			{
				return this._Product;
			}
			set
			{
				this._Product.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_Product(Product entity)
		{
			this.SendPropertyChanging();
			entity.Brands = this;
		}
		
		private void detach_Product(Product entity)
		{
			this.SendPropertyChanging();
			entity.Brands = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Category")]
	public partial class Category : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _CategoryID;
		
		private string _CategoryName;
		
		private EntitySet<Product> _Product;
		
    #region 可扩展性方法定义
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnCategoryIDChanging(string value);
    partial void OnCategoryIDChanged();
    partial void OnCategoryNameChanging(string value);
    partial void OnCategoryNameChanged();
    #endregion
		
		public Category()
		{
			this._Product = new EntitySet<Product>(new Action<Product>(this.attach_Product), new Action<Product>(this.detach_Product));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CategoryID", DbType="VarChar(50) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string CategoryID
		{
			get
			{
				return this._CategoryID;
			}
			set
			{
				if ((this._CategoryID != value))
				{
					this.OnCategoryIDChanging(value);
					this.SendPropertyChanging();
					this._CategoryID = value;
					this.SendPropertyChanged("CategoryID");
					this.OnCategoryIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CategoryName", DbType="VarChar(50)")]
		public string CategoryName
		{
			get
			{
				return this._CategoryName;
			}
			set
			{
				if ((this._CategoryName != value))
				{
					this.OnCategoryNameChanging(value);
					this.SendPropertyChanging();
					this._CategoryName = value;
					this.SendPropertyChanged("CategoryName");
					this.OnCategoryNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Category_Product", Storage="_Product", ThisKey="CategoryID", OtherKey="CategoryID")]
		public EntitySet<Product> Product
		{
			get
			{
				return this._Product;
			}
			set
			{
				this._Product.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_Product(Product entity)
		{
			this.SendPropertyChanging();
			entity.Category = this;
		}
		
		private void detach_Product(Product entity)
		{
			this.SendPropertyChanging();
			entity.Category = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Product")]
	public partial class Product : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _ProductID;
		
		private string _CategoryID;
		
		private string _BrandID;
		
		private string _ProductName;
		
		private string _ProductSize;
		
		private decimal _ProductPrice;
		
		private string _ProductColor;
		
		private EntityRef<Brands> _Brands;
		
		private EntityRef<Category> _Category;
		
    #region 可扩展性方法定义
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnProductIDChanging(string value);
    partial void OnProductIDChanged();
    partial void OnCategoryIDChanging(string value);
    partial void OnCategoryIDChanged();
    partial void OnBrandIDChanging(string value);
    partial void OnBrandIDChanged();
    partial void OnProductNameChanging(string value);
    partial void OnProductNameChanged();
    partial void OnProductSizeChanging(string value);
    partial void OnProductSizeChanged();
    partial void OnProductPriceChanging(decimal value);
    partial void OnProductPriceChanged();
    partial void OnProductColorChanging(string value);
    partial void OnProductColorChanged();
    #endregion
		
		public Product()
		{
			this._Brands = default(EntityRef<Brands>);
			this._Category = default(EntityRef<Category>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProductID", DbType="VarChar(50) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string ProductID
		{
			get
			{
				return this._ProductID;
			}
			set
			{
				if ((this._ProductID != value))
				{
					this.OnProductIDChanging(value);
					this.SendPropertyChanging();
					this._ProductID = value;
					this.SendPropertyChanged("ProductID");
					this.OnProductIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CategoryID", DbType="VarChar(50)")]
		public string CategoryID
		{
			get
			{
				return this._CategoryID;
			}
			set
			{
				if ((this._CategoryID != value))
				{
					if (this._Category.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnCategoryIDChanging(value);
					this.SendPropertyChanging();
					this._CategoryID = value;
					this.SendPropertyChanged("CategoryID");
					this.OnCategoryIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BrandID", DbType="VarChar(50)")]
		public string BrandID
		{
			get
			{
				return this._BrandID;
			}
			set
			{
				if ((this._BrandID != value))
				{
					if (this._Brands.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnBrandIDChanging(value);
					this.SendPropertyChanging();
					this._BrandID = value;
					this.SendPropertyChanged("BrandID");
					this.OnBrandIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProductName", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string ProductName
		{
			get
			{
				return this._ProductName;
			}
			set
			{
				if ((this._ProductName != value))
				{
					this.OnProductNameChanging(value);
					this.SendPropertyChanging();
					this._ProductName = value;
					this.SendPropertyChanged("ProductName");
					this.OnProductNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProductSize", DbType="VarChar(20)")]
		public string ProductSize
		{
			get
			{
				return this._ProductSize;
			}
			set
			{
				if ((this._ProductSize != value))
				{
					this.OnProductSizeChanging(value);
					this.SendPropertyChanging();
					this._ProductSize = value;
					this.SendPropertyChanged("ProductSize");
					this.OnProductSizeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProductPrice", DbType="Decimal(10,2) NOT NULL")]
		public decimal ProductPrice
		{
			get
			{
				return this._ProductPrice;
			}
			set
			{
				if ((this._ProductPrice != value))
				{
					this.OnProductPriceChanging(value);
					this.SendPropertyChanging();
					this._ProductPrice = value;
					this.SendPropertyChanged("ProductPrice");
					this.OnProductPriceChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProductColor", DbType="VarChar(20)")]
		public string ProductColor
		{
			get
			{
				return this._ProductColor;
			}
			set
			{
				if ((this._ProductColor != value))
				{
					this.OnProductColorChanging(value);
					this.SendPropertyChanging();
					this._ProductColor = value;
					this.SendPropertyChanged("ProductColor");
					this.OnProductColorChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Brands_Product", Storage="_Brands", ThisKey="BrandID", OtherKey="BrandID", IsForeignKey=true)]
		public Brands Brands
		{
			get
			{
				return this._Brands.Entity;
			}
			set
			{
				Brands previousValue = this._Brands.Entity;
				if (((previousValue != value) 
							|| (this._Brands.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Brands.Entity = null;
						previousValue.Product.Remove(this);
					}
					this._Brands.Entity = value;
					if ((value != null))
					{
						value.Product.Add(this);
						this._BrandID = value.BrandID;
					}
					else
					{
						this._BrandID = default(string);
					}
					this.SendPropertyChanged("Brands");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Category_Product", Storage="_Category", ThisKey="CategoryID", OtherKey="CategoryID", IsForeignKey=true)]
		public Category Category
		{
			get
			{
				return this._Category.Entity;
			}
			set
			{
				Category previousValue = this._Category.Entity;
				if (((previousValue != value) 
							|| (this._Category.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Category.Entity = null;
						previousValue.Product.Remove(this);
					}
					this._Category.Entity = value;
					if ((value != null))
					{
						value.Product.Add(this);
						this._CategoryID = value.CategoryID;
					}
					else
					{
						this._CategoryID = default(string);
					}
					this.SendPropertyChanged("Category");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
