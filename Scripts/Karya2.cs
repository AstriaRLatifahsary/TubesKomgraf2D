using Godot;
using System;
using System.Collections.Generic;

public partial class Karya2 : Node2D
{
	private BentukDasar _bentukDasar;
	private List<List<Vector2>> _trapesiumBertingkat;
	private List<Vector2> _persegiPanjang;
	private List<Vector2> _segitigaAtas;
	
	private List<Bunga> _bungaList;
	
	private List<List<Vector2>> _motifEndekList;
	private Vector2 _offset = Vector2.Zero;
	private float _amplitudo = 50f;
	private float _frekuensiGerakan = 0.5f;
	private float _time;
	
	private List<List<Vector2>> _sateLilitList;
	private float _elapsedTime = 0f;
	
	private class MotifItem
	{
		public List<Vector2> Shape;
		public float TampilPadaDetik;
	}
	private List<MotifItem> _motifItems;
	private float _totalDurasi = 0f; // waktu total animasi muncul

	public override void _Ready()
	{
		_bentukDasar = new BentukDasar();
		_trapesiumBertingkat = new List<List<Vector2>>();
		_bungaList = new List<Bunga>();

		InisialisasiPura();

		_bungaList.Add(new Bunga(new Vector2(490, 120), 5, Colors.White, _bentukDasar));
		_bungaList.Add(new Bunga(new Vector2(370, 100), 5, Colors.White, _bentukDasar));
		_bungaList.Add(new Bunga(new Vector2(250, 120), 5, Colors.White, _bentukDasar));
		_bungaList.Add(new Bunga(new Vector2(130, 100), 5, Colors.White, _bentukDasar));
		_bungaList.Add(new Bunga(new Vector2(900, 120), 5, Colors.White, _bentukDasar));
		_bungaList.Add(new Bunga(new Vector2(1020, 100), 5, Colors.White, _bentukDasar));
		_bungaList.Add(new Bunga(new Vector2(1140, 120), 5, Colors.White, _bentukDasar));
		_bungaList.Add(new Bunga(new Vector2(1260, 100), 5, Colors.White, _bentukDasar));
	
		InisialisasiMotifEndek();
		SetProcess(true); // Aktifkan update per frame
	
		// Posisi sate lilit
		_sateLilitList = new List<List<Vector2>>();
		_sateLilitList.AddRange(SateLilit(new Vector2(175, 650)));
		_sateLilitList.AddRange(SateLilit(new Vector2(1075, 650)));
		SetProcess(true); // Aktifkan update waktu
		
		InisialisasiPolengDenganMotif();
	}

	public override void _Process(double delta)
	{
		_time += (float)delta;
		QueueRedraw();
		
		// Update posisi X dan Y untuk gerakan diagonal
		_offset = TriangleMovement((float)Time.GetTicksMsec() / 1000f, _frekuensiGerakan, _amplitudo);
		QueueRedraw(); // Memicu _Draw() dipanggil ulang
		
		_elapsedTime += (float)delta;
		QueueRedraw();
	}

	public override void _Draw()
	{
		// Animasi pulau bernafas
		float pulauScale = 1.0f + 0.03f * Mathf.Sin(_time * 2.0f); // Lebih halus dari pura
		Vector2 pulauCenter = new Vector2(695, 640); // Titik tengah pulau

		DrawWithTransform(pulauCenter, pulauScale, () =>
		{
			DrawPolyline(GeneratePulauCustom().ToArray(), Colors.White, 1.5f, true);
		});
		
		// Animasi nafas pura
		float scaleFactor = 1.0f + 0.05f * Mathf.Sin(_time * 2.0f);
		Vector2 scaleOrigin = new Vector2(695, 500);
		DrawWithTransform(scaleOrigin, scaleFactor, () =>
		{
			DrawPolygonLines(_persegiPanjang);
			foreach (var trapesium in _trapesiumBertingkat)
			{
				DrawPolygonLines(trapesium);
			}
			DrawPolygonLines(_segitigaAtas);
		});

		// Animasi bunga berputar dan berdenyut lalu memudar
		foreach (var bunga in _bungaList)
		{
			float scale = 1.0f + 0.1f * Mathf.Sin(_time * 3.0f); // efek denyut
			float alpha = 0.5f + 0.5f * Mathf.Sin(_time * 2.0f); // efek kedip
			Color kelopakColor = new Color(1, 1, 1, alpha); // putih transparan dinamis

			DrawWithRotationAndScale(bunga.Posisi, _time, scale, () =>
			{
				foreach (var kelopak in bunga.GambarBungaEllips())
				{
					for (int i = 0; i < kelopak.Count; i++)
					{
						int nextIndex = (i + 1) % kelopak.Count;
						DrawLine(kelopak[i], kelopak[nextIndex], kelopakColor, 2.0f);
					}
				}
			});
		}
		
		//Animasi Motif Endek
		float waktuSekarang = (float)Time.GetTicksMsec() / 1000f;
		float waveAmplitude = 5f;
		float waveFrequency = 0.05f;

		foreach (var motif in _motifEndekList)
		{
			var motifOffset = new List<Vector2>();
			foreach (var titik in motif)
			{
				// Gerakan segitiga + efek angin gelombang
				Vector2 finalPos = titik + _offset;
				finalPos.Y += Mathf.Sin(titik.X * waveFrequency + waktuSekarang) * waveAmplitude;
				motifOffset.Add(finalPos);
			}
			DrawPolygonLines(motifOffset, 1.5f);
		}
		
		//Animasi Sate Lilit
		int groupIndex = 0;
		for (int i = 0; i < _sateLilitList.Count; i += 2)
		{
			var tusuk = _sateLilitList[i];
			var lilit = _sateLilitList[i + 1];

			// Vertikal bergantian arah (fase per sate)
			float offsetY = Mathf.Sin(_elapsedTime * 2f + groupIndex * 1.5f) * 5f;

			var tusukAnim = OffsetPoints(tusuk, new Vector2(0, offsetY));
			var lilitAnim = OffsetPoints(lilit, new Vector2(0, offsetY));

			DrawPolygonLines(tusukAnim, 2f);
			DrawPolygonLines(lilitAnim, 2f);

			groupIndex++;
		}
		
		//Animasi Motif Poleng
		if (_motifItems == null)
			return;

		foreach (var item in _motifItems)
		{
			if (_elapsedTime >= item.TampilPadaDetik)
			{
				if (item.Shape.Count > 1)
				{
					Vector2[] shapeToDraw = new Vector2[item.Shape.Count];

					// Terapkan efek bergelombang hanya setelah semua muncul
					bool aktifkanGelombang = _elapsedTime >= _totalDurasi;

					for (int i = 0; i < item.Shape.Count; i++)
					{
						Vector2 p = item.Shape[i];

						if (aktifkanGelombang)
						{
							// Efek angin bergelombang
							float offsetY = Mathf.Sin((p.X * 0.05f) + (_elapsedTime * 2f)) * 3f;
							shapeToDraw[i] = new Vector2(p.X, p.Y + offsetY);
						}
						else
						{
							shapeToDraw[i] = p;
						}
					}

					DrawPolyline(shapeToDraw, Colors.White, 1.01f);
				}
			}
		}
	}

	private void DrawPolygonLines(List<Vector2> points, float thickness = 2.0f)
	{
		for (int i = 0; i < points.Count; i++)
		{
			int nextIndex = (i + 1) % points.Count;
			DrawLine(points[i], points[nextIndex], Colors.White, thickness);
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

	private void DrawWithTransform(Vector2 origin, float scale, Action drawAction)
	{
		var xform = Transform2D.Identity.Scaled(new Vector2(scale, scale)).Translated(origin * (1 - scale));
		DrawSetTransformMatrix(xform);
		drawAction.Invoke();
		DrawSetTransformMatrix(Transform2D.Identity);
	}

	private void DrawWithRotation(Vector2 origin, float angle, Action drawAction)
	{
		var xform = Transform2D.Identity.Translated(-origin).Rotated(angle).Translated(origin);
		DrawSetTransformMatrix(xform);
		drawAction.Invoke();
		DrawSetTransformMatrix(Transform2D.Identity);
	}

	private void DrawWithRotationAndScale(Vector2 origin, float angle, float scale, Action drawAction)
	{
		var xform = Transform2D.Identity
			.Translated(-origin)
			.Rotated(angle)
			.Scaled(new Vector2(scale, scale))
			.Translated(origin);
		DrawSetTransformMatrix(xform);
		drawAction.Invoke();
		DrawSetTransformMatrix(Transform2D.Identity);
	}

	// Inisialisasi motif endek
	private void InisialisasiMotifEndek()
	{
		Vector2 posisiEndek = new Vector2(695, 300);
		_motifEndekList = new List<List<Vector2>>();

		int jumlahBaris = 5;
		int jumlahKolom = 7;
		float jarakX = 40;
		float jarakY = 45;
		Vector2 posisiAwal = new Vector2(90, 300);

		for (int baris = 0; baris < jumlahBaris; baris++)
		{
			for (int kolom = 0; kolom < jumlahKolom; kolom++)
			{
				float offsetX = (baris % 2 == 1) ? jarakX / 2 : 0;
				Vector2 posisi = posisiAwal + new Vector2(kolom * jarakX + offsetX, baris * jarakY);

				if (baris % 2 == 0)
				{
					if (kolom % 2 == 0)
					{
						var outline = _bentukDasar.PolaEndek(posisi, 60, 90);
						var kecil = _bentukDasar.BelahKetupatKecil(posisi, 20, 25);
						_motifEndekList.Add(outline);
						_motifEndekList.Add(kecil);
					}
					else
					{
						var kecil = _bentukDasar.BelahKetupatKecil(posisi, 20, 25);
						_motifEndekList.Add(kecil);
					}
				}
				else
				{
					var kecil = _bentukDasar.BelahKetupatKecil(posisi, 15, 20);
					_motifEndekList.Add(kecil);
				}
			}
		}

		float maxHalfLebar = 30f;
		float maxHalfTinggi = 45f;

		Vector2 posisiKiriAtas = posisiAwal;
		Vector2 posisiKananBawah = posisiAwal + new Vector2((jumlahKolom - 1) * jarakX, (jumlahBaris - 1) * jarakY);

		if ((jumlahBaris - 1) % 2 == 1)
		{
			posisiKananBawah.X += jarakX / 2;
		}

		float outlineX = posisiKiriAtas.X - maxHalfLebar;
		float outlineY = posisiKiriAtas.Y - maxHalfTinggi;
		float outlineLebar = (posisiKananBawah.X - posisiKiriAtas.X) + maxHalfLebar * 2;
		float outlineTinggi = (posisiKananBawah.Y - posisiKiriAtas.Y) + maxHalfTinggi * 2;

		var outlineMotif = _bentukDasar.PersegiPanjang(outlineX, outlineY, outlineLebar, outlineTinggi);
		_motifEndekList.Add(outlineMotif);
	}

	private List<Vector2> ReflectShapeVertically(List<Vector2> shape, float mirrorY)
	{
		List<Vector2> reflected = new();
		foreach (var point in shape)
		{
			var dy = point.Y - mirrorY;
			reflected.Add(new Vector2(point.X, mirrorY - dy));
		}
		return reflected;
	}
	
	private Vector2 TriangleMovement(float time, float frequency, float amplitude)
	{
		Vector2 pointA = new Vector2(0, amplitude);
		Vector2 pointB = new Vector2(amplitude, -amplitude);
		Vector2 pointC = new Vector2(-amplitude, -amplitude);

		float lenAB = pointA.DistanceTo(pointB);
		float lenBC = pointB.DistanceTo(pointC);
		float lenCA = pointC.DistanceTo(pointA);

		float totalLen = lenAB + lenBC + lenCA;

		float durAB = lenAB / totalLen;
		float durBC = lenBC / totalLen;
		float durCA = lenCA / totalLen;

		float period = 2f / frequency;
		float t = (time % period) / period;

		if (t < durAB)
		{
			float localT = t / durAB;
			return pointA.Lerp(pointB, localT);
		}
		else if (t < durAB + durBC)
		{
			float localT = (t - durAB) / durBC;
			return pointB.Lerp(pointC, localT);
		}
		else
		{
			float localT = (t - durAB - durBC) / durCA;
			return pointC.Lerp(pointA, localT);
		}
	}

	private Vector2 Lerp(Vector2 a, Vector2 b, float t)
	{
		return a + (b - a) * t;
	}
	
	private List<Vector2> OffsetPoints(List<Vector2> points, Vector2 offset)
	{
		var hasil = new List<Vector2>();
		foreach (var p in points)
		{
			hasil.Add(p + offset);
		}
		return hasil;
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
		_motifItems = new List<MotifItem>();

		int baris = 5;
		int kolom = 6;
		float size = 50f;
		Vector2 awal = new Vector2(1050, 248);

		float delay = 0f;
		float delayPerKotak = 0.1f;

		for (int i = 0; i < baris; i++)
		{
			for (int j = 0; j < kolom; j++)
			{
				float x = awal.X + j * size;
				float y = awal.Y + i * size;

				var kotak = BuatPersegi(x, y, size);
				_motifItems.Add(new MotifItem { Shape = kotak, TampilPadaDetik = delay });

				bool putih = (i + j) % 2 == 0;
				if (putih)
				{
					var bunga = BuatMotifBungaKlasik(x + size / 2, y + size / 2, 4);
					foreach (var b in bunga)
					{
						_motifItems.Add(new MotifItem { Shape = b, TampilPadaDetik = delay + 0.03f });
					}
				}

				delay += delayPerKotak;
			}
		}

		_totalDurasi = delay; // ⏱️ simpan durasi total supaya tahu kapan semua sudah muncul
	}

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

	private List<List<Vector2>> BuatMotifBungaKlasik(float centerX, float centerY, float spacing)
	{
		var result = new List<List<Vector2>>();
		float s = 3f;

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
