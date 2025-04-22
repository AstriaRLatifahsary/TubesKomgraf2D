using Godot;
using System;
using System.Collections.Generic;

public partial class Karya1 : Node2D
{
	private BentukDasar _bentukDasar;
	private List<List<Vector2>> _trapesiumBertingkat;
	private List<Vector2> _persegiPanjang;
	private List<Vector2> _segitigaAtas;
	private List<Bunga> _bungaList;
	private List<List<Vector2>> _motifEndekList;
	private List<List<Vector2>> _sateLilitList;
	private List<List<Vector2>> _motifPolengList;

	public override void _Ready()
	{
		_bentukDasar = new BentukDasar();
		_trapesiumBertingkat = new List<List<Vector2>>();
		_bungaList = new List<Bunga>();

		InisialisasiPura();

		//posisi bunga kamboja
		_bungaList.Add(new Bunga(new Vector2(490, 120), 5, Colors.White, _bentukDasar));
		_bungaList.Add(new Bunga(new Vector2(370, 100), 5, Colors.White, _bentukDasar));
		_bungaList.Add(new Bunga(new Vector2(250, 120), 5, Colors.White, _bentukDasar));
		_bungaList.Add(new Bunga(new Vector2(130, 100), 5, Colors.White, _bentukDasar));
		_bungaList.Add(new Bunga(new Vector2(900, 120), 5, Colors.White, _bentukDasar));
		_bungaList.Add(new Bunga(new Vector2(1020, 100), 5, Colors.White, _bentukDasar));
		_bungaList.Add(new Bunga(new Vector2(1140, 120), 5, Colors.White, _bentukDasar));
		_bungaList.Add(new Bunga(new Vector2(1260, 100), 5, Colors.White, _bentukDasar));
		
		InisialisasiMotifEndek();
		
		// Posisi sate lilit
		_sateLilitList = new List<List<Vector2>>();
		_sateLilitList.AddRange(SateLilit(new Vector2(175, 650)));
		_sateLilitList.AddRange(SateLilit(new Vector2(1075, 650)));
		
		InisialisasiPolengDenganMotif();
	}

	public override void _Draw()
	{
		// Pulau penopang pura
		DrawPolyline(GeneratePulauCustom().ToArray(), Colors.White, 1.5f, true);
		
		// Pura
		DrawPolygonLines(_persegiPanjang, Colors.White);

		foreach (var trapesium in _trapesiumBertingkat)
		{
			DrawPolygonLines(trapesium, Colors.White);
		}

		DrawPolygonLines(_segitigaAtas, Colors.White);

		//bunga kamboja
		foreach (var bunga in _bungaList)
		{
			GambarBunga(bunga);
		}
		
		//motif kain Endek
		foreach (var motif in _motifEndekList)
		{
			DrawPolygonLines(motif, Colors.White, 1.5f);
		}

		//sate lilit
		foreach (var bentuk in _sateLilitList)
			{
				DrawPolygonLines(bentuk, Colors.White, 2f);
			}
		
		if (_motifPolengList == null)
			return;

		foreach (var shape in _motifPolengList)
		{
			if (shape.Count > 1)
			{
				DrawPolyline(shape.ToArray(), Colors.White, 1.01f);
			}
		}
	}
	
	private void DrawPolygonLines(List<Vector2> points, Color color, float thickness = 2.0f)
	{
		for (int i = 0; i < points.Count; i++)
		{
			int nextIndex = (i + 1) % points.Count;
			DrawLine(points[i], points[nextIndex], color, thickness);
		}
	}
	
	private void InisialisasiPura()
	{
		_persegiPanjang = _bentukDasar.PersegiPanjang(600, 495, 200, 80);

		_trapesiumBertingkat = _bentukDasar.TrapesiumBertingkat(
			jumlah: 11,
			posisiAwalY: 460,
			panjangAtasAkhir: 100,
			panjangBawahAwal: 400,
			tinggiTrapesium: 35,
			centerX: 695
		);

		_segitigaAtas = _bentukDasar.SegitigaSamaKaki(new Vector2(695, 110), 90, 40);
	}

	private void GambarBunga(Bunga bunga)
	{
		foreach (var kelopak in bunga.GambarBungaEllips())
		{
			DrawPolygonLines(kelopak, bunga.Warna, 2.0f);
		}
	}
	
	private List<Vector2> GeneratePulauCustom()
	{
		Vector2 center = new Vector2(695, 603); // Titik tengah bawah pura
		float radiusX = 300; // Lebar pulau
		float radiusY = 30;  // Ketinggian elips
		int segments = 50;

		List<Vector2> points = new List<Vector2>();

		// Setengah elips bawah (dari kiri ke kanan)
		for (int i = 0; i <= segments; i++)
		{
			float t = (float)i / segments;
			float angle = Mathf.Pi + t * Mathf.Pi; // Dari 180° ke 360°
			float x = center.X + radiusX * Mathf.Cos(angle);
			float y = center.Y + radiusY * Mathf.Sin(angle);
			points.Add(new Vector2(x, y));
		}

		// Tutup jalur kembali ke titik awal agar bisa digunakan dengan `closed: true`
		points.Add(points[0]);

		return points;
	}

	private void InisialisasiMotifEndek()
	{
		Vector2 posisiEndek = new Vector2(695, 300); 
		//float lebarEndek = 80;
		//float tinggiEndek = 120;

		_motifEndekList = new List<List<Vector2>>();

		int jumlahBaris = 5;
		int jumlahKolom = 7;
		float jarakX = 40;
		float jarakY = 45;
		Vector2 posisiAwal = new Vector2(42, 300);

		for (int baris = 0; baris < jumlahBaris; baris++)
		{
			for (int kolom = 0; kolom < jumlahKolom; kolom++)
			{
				float offsetX = (baris % 2 == 1) ? jarakX / 2 : 0; // geser kanan setengah jarakX untuk baris ganjil
				Vector2 posisi = posisiAwal + new Vector2(kolom * jarakX + offsetX, baris * jarakY);

				if (baris % 2 == 0) // Baris 0, 2, dst
				{
					if (kolom % 2 == 0)
					{
						// Ketupat besar isi kecil
						var outline = _bentukDasar.PolaEndek(posisi, 60, 90);
						var kecil = _bentukDasar.BelahKetupatKecil(posisi, 20, 25);
						_motifEndekList.Add(outline);
						_motifEndekList.Add(kecil);
					}
					else
					{
						// Hanya ketupat kecil
						var kecil = _bentukDasar.BelahKetupatKecil(posisi, 20, 25);
						_motifEndekList.Add(kecil);
					}
				}
				else // Baris ganjil
				{
					var kecil = _bentukDasar.BelahKetupatKecil(posisi, 15, 20);
					_motifEndekList.Add(kecil);
				}
			}
		}
		
		// Tambahkan batas maksimal ketupat besar
		float maxHalfLebar = 30f; // setengah dari 60
		float maxHalfTinggi = 45f; // setengah dari 90

		// Ambil posisi motif pertama (paling kiri atas) dan terakhir (kanan bawah)
		Vector2 posisiKiriAtas = posisiAwal;
		Vector2 posisiKananBawah = posisiAwal + new Vector2((jumlahKolom - 1) * jarakX, (jumlahBaris - 1) * jarakY);

		// Jika baris terakhir ganjil (bergeser ke kanan), tambahkan offsetX
		if ((jumlahBaris - 1) % 2 == 1)
		{
			posisiKananBawah.X += jarakX / 2;
		}

		// Hitung total area yang benar-benar dibutuhkan
		float outlineX = posisiKiriAtas.X - maxHalfLebar;
		float outlineY = posisiKiriAtas.Y - maxHalfTinggi;
		float outlineLebar = (posisiKananBawah.X - posisiKiriAtas.X) + maxHalfLebar * 2;
		float outlineTinggi = (posisiKananBawah.Y - posisiKiriAtas.Y) + maxHalfTinggi * 2;

		// Buat persegi panjang outline
		var outlineMotif = _bentukDasar.PersegiPanjang(outlineX, outlineY, outlineLebar, outlineTinggi);
		_motifEndekList.Add(outlineMotif);
	}

	private List<List<Vector2>> SateLilit(
		Vector2 posisi,
		int jumlahTusuk = 4,
		float panjangTusuk = 60,
		float lebarTusuk = 5,
		float panjangLilit = 40,
		float lebarLilit = 15,
		float faktorSpasi = 2.2f
	)
	{
		var hasil = new List<List<Vector2>>();
		float spasiAntarTusuk = lebarLilit * faktorSpasi;

		for (int i = 0; i < jumlahTusuk; i++)
		{
			float offsetX = i * spasiAntarTusuk;
			Vector2 posisiTusuk = posisi + new Vector2(offsetX, 0);

			// Geser ke kiri agar center
			float tusukX = posisiTusuk.X - lebarTusuk / 2f;
			float tusukY = posisiTusuk.Y;

			var tusuk = _bentukDasar.PersegiPanjang(tusukX, tusukY, lebarTusuk, panjangTusuk);
			hasil.Add(tusuk);

			// Elips berada di atas tusuk
			Vector2 posisiLilit = posisiTusuk + new Vector2(0, -panjangLilit / 2);
			var lilit = _bentukDasar.Elips(posisiLilit, (int)lebarLilit, (int)panjangLilit);
			hasil.Add(lilit);
		}

		return hasil;
	}

	private void InisialisasiPolengDenganMotif()
	{
		_motifPolengList = new List<List<Vector2>>();

		int baris = 5;
		int kolom = 6;
		float size = 50f;
		Vector2 awal = new Vector2(1050, 248);

		for (int i = 0; i < baris; i++)
		{
			for (int j = 0; j < kolom; j++)
			{
				float x = awal.X + j * size;
				float y = awal.Y + i * size;

				// Kotak outline
				var kotak = BuatPersegi(x, y, size);
				_motifPolengList.Add(kotak);

				// Pola checkerboard
				bool putih = (i + j) % 2 == 0;

				if (putih)
				{
					// Tambahkan bunga pixel outline di tengah kotak putih
					var bunga = BuatMotifBungaKlasik(x + size / 2, y + size / 2, 4);
					_motifPolengList.AddRange(bunga);
				}
			}
		}
	}

	// Outline persegi
	private List<Vector2> BuatPersegi(float x, float y, float sisi)
	{
		return new List<Vector2>
		{
			new Vector2(x, y),
			new Vector2(x + sisi, y),
			new Vector2(x + sisi, y + sisi),
			new Vector2(x, y + sisi),
			new Vector2(x, y) // menutup kotak
		};
	}

	// Buat bunga pixel (outline kotak kecil membentuk bunga)
	private List<List<Vector2>> BuatMotifBungaKlasik(float centerX, float centerY, float spacing)
	{
		var result = new List<List<Vector2>>();
		float s = 3f; // ukuran kotak kecil (pixel)

		// Representasi grid 9x9 motif bunga (1 = kotak diisi, 0 = kosong)
		int[,] grid = new int[,]
		{
			{0,0,0,0,1,0,0,0,0},
			{0,0,0,1,0,1,0,0,0},
			{0,0,1,0,1,0,1,0,0},
			{0,1,0,1,1,1,0,1,0},
			{0,0,1,1,1,1,1,0,0},
			{0,1,0,1,1,1,0,1,0},
			{0,0,1,0,1,0,1,0,0},
			{0,0,0,1,0,1,0,0,0},
			{0,0,0,0,1,0,0,0,0}
		};

		int rows = grid.GetLength(0);
		int cols = grid.GetLength(1);

		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				if (grid[i, j] == 1)
				{
					// Hitung posisi berdasarkan pusat dan offset grid
					float x = centerX + (j - cols / 2) * spacing - s / 2;
					float y = centerY + (i - rows / 2) * spacing - s / 2;

					result.Add(BuatPersegi(x, y, s));
				}
			}
		}

		return result;
	}

	public override void _ExitTree()
	{
		_bentukDasar?.Dispose();
		_bentukDasar = null;
	}
}
