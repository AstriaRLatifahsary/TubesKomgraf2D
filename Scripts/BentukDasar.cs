namespace Godot;

using Godot;
using System;
using System.Collections.Generic;

public partial class BentukDasar : RefCounted, IDisposable
{
	private Primitif _primitif = new Primitif();

	public List<Vector2> Margin()
	{
		return CheckPrimitifAndCall(() => _primitif.Margin());
	}

	public List<Vector2> Persegi(float x, float y, float ukuran)
	{
		return CheckPrimitifAndCall(() => _primitif.Persegi(x, y, ukuran));
	}

	public List<Vector2> PersegiPanjang(float x, float y, float panjang, float lebar)
	{
		return CheckPrimitifAndCall(() => _primitif.PersegiPanjang(x, y, panjang, lebar));
	}

	public List<Vector2> SegitigaSamaKaki(Vector2 center, float baseLength, float height)
	{
		float halfBase = baseLength / 2;
		return new List<Vector2>
		{
			new Vector2(center.X - halfBase, center.Y),
			new Vector2(center.X + halfBase, center.Y),
			new Vector2(center.X, center.Y - height)
		};
	}

	public List<Vector2> SegitigaSiku(Vector2 titikAwal, int alas, int tinggi)
	{
		return CheckPrimitifAndCall(() => _primitif.SegitigaSiku(titikAwal, alas, tinggi));
	}

	public List<Vector2> TrapesiumSiku(Vector2 titikAwal, int panjangAtas, int panjangBawah, int tinggi)
	{
		return CheckPrimitifAndCall(() => _primitif.TrapesiumSiku(titikAwal, panjangAtas, panjangBawah, tinggi));
	}

	public List<Vector2> TrapesiumSamaKaki(Vector2 puncakTengah, int panjangAtas, int panjangBawah, int tinggi)
	{
		float halfAtas = panjangAtas / 2.0f;
		float halfBawah = panjangBawah / 2.0f;
		float bawahY = puncakTengah.Y + tinggi;

		var p1 = new Vector2(puncakTengah.X - halfAtas, puncakTengah.Y);      // kiri atas
		var p2 = new Vector2(puncakTengah.X + halfAtas, puncakTengah.Y);      // kanan atas
		var p3 = new Vector2(puncakTengah.X + halfBawah, bawahY);             // kanan bawah
		var p4 = new Vector2(puncakTengah.X - halfBawah, bawahY);             // kiri bawah

		return new List<Vector2> { p1, p2, p3, p4 };
	}

	public List<Vector2> JajarGenjang(Vector2 titikAwal, int alas, int tinggi, int jarakBeda)
	{
		return CheckPrimitifAndCall(() => _primitif.JajarGenjang(titikAwal, alas, tinggi, jarakBeda));
	}

	public List<Vector2> Elips(Vector2 center, int radiusX, int radiusY)
	{
		List<Vector2> points = new();
		for (float angle = 0; angle < Mathf.Tau; angle += 0.05f)
		{
			float x = center.X + radiusX * Mathf.Cos(angle);
			float y = center.Y + radiusY * Mathf.Sin(angle);
			points.Add(new Vector2(x, y));
		}
		return points;
	}

	public List<Vector2> Lingkaran(Vector2 center, int radius)
	{
		List<Vector2> points = new();
		for (float angle = 0; angle < Mathf.Tau; angle += 0.05f)
		{
			float x = center.X + radius * Mathf.Cos(angle);
			float y = center.Y + radius * Mathf.Sin(angle);
			points.Add(new Vector2(x, y));
		}
		return points;
	}


	// Trapesium bertingkat
	public List<List<Vector2>> TrapesiumBertingkat(
		int jumlah, int posisiAwalY, int panjangAtasAkhir, int panjangBawahAwal, int tinggiTrapesium, int centerX)
	{
		var hasil = new List<List<Vector2>>();

		float panjangBawah = panjangBawahAwal;
		float panjangAtas = panjangBawahAwal * 0.8f;

		float stepAtas = (panjangAtas - panjangAtasAkhir) / jumlah;
		float stepBawah = (panjangBawah - panjangAtasAkhir) / jumlah;

		int posisiY = posisiAwalY;

		for (int i = 0; i < jumlah; i++)
		{
			hasil.Add(Polygon(
				TrapesiumSamaKaki(
					new Vector2(centerX, posisiY),
					(int)panjangAtas,
					(int)panjangBawah,
					tinggiTrapesium
				)
			));

			panjangAtas -= stepAtas;
			panjangBawah -= stepBawah;
			posisiY -= tinggiTrapesium;
		}

		return hasil;
	}
		
	// Pola Kain Endek 
	public List<Vector2> PolaEndek(Vector2 center, float width, float height)
	{
		Vector2 top = center + new Vector2(0, -height / 2);
		Vector2 right = center + new Vector2(width / 2, 0);
		Vector2 bottom = center + new Vector2(0, height / 2);
		Vector2 left = center + new Vector2(-width / 2, 0);

		return Polygon(new List<Vector2> { top, right, bottom, left });
	}	
		
	public List<Vector2> BelahKetupatKecil(Vector2 center, float lebar, float tinggi)
	{
		return new List<Vector2>
		{
			new Vector2(center.X, center.Y - tinggi / 2),  // atas
			new Vector2(center.X + lebar / 2, center.Y),   // kanan
			new Vector2(center.X, center.Y + tinggi / 2),  // bawah
			new Vector2(center.X - lebar / 2, center.Y),   // kiri
		};
	}

	
	// Fungsi Polygon menyambung titik-titik menjadi outline dengan algoritma Bresenham
	public List<Vector2> Polygon(List<Vector2> points)
	{
		List<Vector2> checkResult = NodeUtils.CheckPrimitif(_primitif);
		if (checkResult != null) return checkResult;

		List<Vector2> polygonPoints = new List<Vector2>();
		for (int i = 0; i < points.Count; i++)
		{
			int nextIndex = (i + 1) % points.Count;
			polygonPoints.AddRange(_primitif.LineBresenham(points[i].X, points[i].Y, points[nextIndex].X, points[nextIndex].Y));
		}
		return polygonPoints;
	}

	private List<Vector2> CheckPrimitifAndCall(Func<List<Vector2>> action)
	{
		List<Vector2> checkResult = NodeUtils.CheckPrimitif(_primitif);
		if (checkResult != null) return checkResult;
		return action();
	}

	public new void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected new virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			NodeUtils.DisposeAndNull(_primitif, "_primitif");
		}
	}
}
