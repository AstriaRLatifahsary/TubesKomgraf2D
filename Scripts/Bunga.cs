using Godot;
using System;
using System.Collections.Generic;

public class Bunga
{
	public Vector2 Posisi;
	private int JumlahKelopak;
	public Color Warna;
	private BentukDasar BentukDasar;

	private float waktu = 0.0f;
	private float amplitudo = 10.0f;
	private float frekuensi = 1.0f;
	private float fase;

	public Bunga(Vector2 posisi, int jumlahKelopak, Color warna, BentukDasar bentukDasar)
	{
		Posisi = posisi;
		JumlahKelopak = jumlahKelopak;
		Warna = warna;
		BentukDasar = bentukDasar;
		fase = (float)GD.RandRange(0, Mathf.Pi * 2); // untuk variasi antar bunga
	}

	public void Update(float delta)
	{
		waktu += delta;
	}

	private Vector2 GetPosisiAnimasi()
	{
		float offsetY = Mathf.Sin(waktu * frekuensi + fase) * amplitudo;
		return new Vector2(Posisi.X, Posisi.Y + offsetY);
	}

	public List<List<Vector2>> GambarBungaEllips()
	{
		List<List<Vector2>> hasil = new();
		float angleStep = 360.0f / JumlahKelopak;

		for (int i = 0; i < JumlahKelopak; i++)
		{
			float angle = Mathf.DegToRad(i * angleStep);
			hasil.Add(GambarKelopak(angle));
		}

		hasil.Add(GambarLingkaran(GetPosisiAnimasi(), 15));
		return hasil;
	}

	private List<Vector2> GambarKelopak(float rotationAngle)
	{
		float distance = 25;
		Vector2 kelopakCenter = GetPosisiAnimasi() + new Vector2(0, -distance).Rotated(rotationAngle);
		return GambarEllips(kelopakCenter, 10, 25, rotationAngle);
	}

	private List<Vector2> GambarEllips(Vector2 center, int radiusX, int radiusY, float rotationAngle)
	{
		List<Vector2> points = BentukDasar.Elips(center, radiusX, radiusY);
		return RotasiPolygon(points, center, rotationAngle);
	}

	private List<Vector2> GambarLingkaran(Vector2 center, int radius)
	{
		return BentukDasar.Lingkaran(center, radius);
	}

	private List<Vector2> RotasiPolygon(List<Vector2> points, Vector2 center, float angle)
	{
		List<Vector2> rotatedPoints = new();
		foreach (var point in points)
		{
			Vector2 rotatedPoint = (point - center).Rotated(angle) + center;
			rotatedPoints.Add(rotatedPoint);
		}
		return rotatedPoints;
	}
}
