using Godot;
using System;
using System.Collections.Generic;

public partial class Karya3 : Node2D
{
	private BentukDasar _bentukDasar;
	private List<List<Vector2>> _trapesiumBertingkat;
	private List<Vector2> _persegiPanjang;
	private List<Vector2> _segitigaAtas;
	private float _time; //pura,bunga,endek

	private List<Bunga> _bungaList;

	private List<List<Vector2>> _motifEndekList;
	private Vector2 _offset = Vector2.Zero;
	private float _amplitudo = 50f;
	private float _frekuensiGerakan = 0.5f;

	private List<List<Vector2>> _sateLilitList;
	private float _elapsedTime = 0f; //sate,poleng

	private class MotifItem
	{
		public List<Vector2> Shape;
		public float TampilPadaDetik;
		public Color Warna;
	}
	private List<MotifItem> _motifItems;
	private float _totalDurasi = 0f;

	public override void _Ready()
	{
		_bentukDasar = new BentukDasar();
		_trapesiumBertingkat = new List<List<Vector2>>();
		InisialisasiPura();

		// Posisi bunga
		_bungaList = new List<Bunga>();
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

		// Update posisi X dan Y untuk gerakan diagonal (motif_endek)
		_offset = TriangleMovement((float)Time.GetTicksMsec() / 1000f, _frekuensiGerakan, _amplitudo);
		QueueRedraw(); // Memicu _Draw() dipanggil ulang

		_elapsedTime += (float)delta; //sate,poleng
		QueueRedraw();
	}

	public override void _Draw()
	{
		DrawRect(new Rect2(Vector2.Zero, GetViewportRect().Size), new Color(0.45f, 0.65f, 0.85f), true);
		
		// Animasi pulau bernafas
		float pulauScale = 1.0f + 0.03f * Mathf.Sin(_time * 2.0f);
		Vector2 pulauCenter = new Vector2(695, 640);

		// Warna coklat untuk pulau
		Color pulauColor = new Color(0.4f, 0.2f, 0.05f);

		DrawWithTransform(pulauCenter, pulauScale, () =>
		{
			DrawPolygon(GeneratePulauCustom().ToArray(), new Color[] { pulauColor });
			DrawPolyline(GeneratePulauCustom().ToArray(), Colors.Green, 1.5f, true);
		});

		// Animasi nafas pura
		float scaleFactor = 1.0f + 0.05f * Mathf.Sin(_time * 2.0f);
		Vector2 scaleOrigin = new Vector2(695, 500);

		// Warna abu-abu tua untuk pura
		Color fillColor = new Color(0.4f, 0.4f, 0.4f);

		DrawWithTransform(scaleOrigin, scaleFactor, () =>
		{
			// Persegi panjang dasar
			DrawPolygon(_persegiPanjang.ToArray(), new Color[] { fillColor });
			DrawPolygonLines(_persegiPanjang);

			// Trapesium bertingkat
			foreach (var trapesium in _trapesiumBertingkat)
			{
				DrawPolygon(trapesium.ToArray(), new Color[] { fillColor });
				DrawPolygonLines(trapesium);
			}

			// Segitiga atas
			DrawPolygon(_segitigaAtas.ToArray(), new Color[] { fillColor });
			DrawPolygonLines(_segitigaAtas);
		});

		// Animasi bunga berputar dan berdenyut lalu memudar
		foreach (var bunga in _bungaList)
		{
			float scale = 1.0f + 0.1f * Mathf.Sin(_time * 3.0f); // efek denyut
			float alpha = 0.5f + 0.5f * Mathf.Sin(_time * 2.0f); // efek kedip
			Color kelopakColor = new Color(1, 1, 1, alpha); // putih transparan dinamis
			Color lingkaranColor = new Color(1, 1, 0, alpha); // kuning transparan dinamis

			DrawWithRotationAndScale(bunga.Posisi, _time, scale, () =>
			{
				var bentukBunga = bunga.GambarBungaEllips();

				// Kelopak (semua kecuali elemen terakhir)
				for (int i = 0; i < bentukBunga.Count - 1; i++)
				{
					var kelopak = bentukBunga[i];
					DrawPolygon(kelopak.ToArray(), new Color[] { kelopakColor });
				}

				// Lingkaran tengah (elemen terakhir)
				var lingkaran = bentukBunga[^1];
				DrawPolygon(lingkaran.ToArray(), new Color[] { lingkaranColor });
			});
		}

		// Animasi Motif Endek tertiup angin berjalan membentuk pola segitiga
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

			// Gambar isi motif dengan warna abu-abu transparan
			Color EndekColor = new Color(0, 0, 0, 0.3f);
			DrawPolygon(motifOffset.ToArray(), new Color[] { EndekColor });
			DrawPolygonLinesEndek(motifOffset, 3f, new Color(0.6f, 0.1f, 0.3f));
		}

		// Animasi Sate Lilit 
		int groupIndex = 0;
		for (int i = 0; i < _sateLilitList.Count; i += 2)
		{
			var tusuk = _sateLilitList[i];
			var lilit = _sateLilitList[i + 1];

			// Vertikal bergantian arah (fase per sate)
			float offsetY = Mathf.Sin(_elapsedTime * 2f + groupIndex * 1.5f) * 5f;

			var tusukAnim = OffsetPoints(tusuk, new Vector2(0, offsetY));
			var lilitAnim = OffsetPoints(lilit, new Vector2(0, offsetY));

			// Gambar batang sate dengan warna hijau
			DrawPolygon(tusukAnim.ToArray(), new Color[] { new Color(0.0f, 1.0f, 0.0f) }); // Hijau sebagai array
			// Gambar lilit sate dengan warna coklat
			DrawPolygon(lilitAnim.ToArray(), new Color[] { new Color(0.6f, 0.3f, 0.1f) }); // Coklat sebagai array

			groupIndex++;
		}

		// Animasi Pola Poleng	
		if (_motifItems == null)
			return;

		foreach (var item in _motifItems)
		{
			if (_elapsedTime >= item.TampilPadaDetik)
			{
				if (item.Shape.Count > 1)
				{
					Vector2[] shapeToDraw = new Vector2[item.Shape.Count];
					bool aktifkanGelombang = _elapsedTime >= _totalDurasi;

					for (int i = 0; i < item.Shape.Count; i++)
					{
						Vector2 p = item.Shape[i];
						if (aktifkanGelombang)
						{
							float offsetY = Mathf.Sin((p.X * 0.05f) + (_elapsedTime * 2f)) * 3f;
							shapeToDraw[i] = new Vector2(p.X, p.Y + offsetY);
						}
						else
						{
							shapeToDraw[i] = p;
						}
					}

					DrawPolygon(shapeToDraw, new Color[] { item.Warna });
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

	private void DrawPolygonLinesEndek(List<Vector2> points, float thickness = 2.0f, Color? lineColor = null)
	{
		Color finalColor = lineColor ?? Colors.White;

		for (int i = 0; i < points.Count; i++)
		{
			int nextIndex = (i + 1) % points.Count;
			DrawLine(points[i], points[nextIndex], finalColor, thickness);
		}
	}
	
	private void DrawPolygon(Vector2[] points, Color[] colors)
	{
		if (points.Length >= 3) // Cek apakah ada cukup titik untuk membentuk poligon
		{
			// Gambar poligon dengan warna yang diberikan
			base.DrawPolygon(points, colors); // Menggunakan array warna
		}
	}

	//PURA
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
		Vector2 center = new Vector2(695, 603);
		float radiusX = 300;
		float radiusY = 30;
		int segments = 50;

		List<Vector2> points = new List<Vector2>();

		for (int i = 0; i <= segments; i++)
		{
			float t = (float)i / segments;
			float angle = Mathf.Pi + t * Mathf.Pi;
			float x = center.X + radiusX * Mathf.Cos(angle);
			float y = center.Y + radiusY * Mathf.Sin(angle);
			points.Add(new Vector2(x, y));
		}

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

	//BUNGA
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

	//MOTIF ENDEK
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
	
	//SATE LILIT
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
	
	//POLA POLENG
	private Color GetKotakColor(List<Vector2> shape)
	{
		int kolom = (int)(shape[0].X / 50);
		int baris = (int)(shape[0].Y / 50);

		if (baris % 2 == 0)
		{
			return (kolom % 2 == 0) ? Colors.DarkGray : Colors.White;
		}
		else
		{
			return (kolom % 2 == 0) ? Colors.Black : Colors.DarkGray;
		}
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

		Color[] warnaBungaGenap = new Color[] { Colors.Green, Colors.Red, Colors.Purple };
		Color warnaBungaGanjil = Colors.Yellow;

		for (int i = 0; i < baris; i++)
		{
			int bungaIndex = 0;

			for (int j = 0; j < kolom; j++)
			{
				float x = awal.X + j * size;
				float y = awal.Y + i * size;

				var kotak = BuatPersegi(x, y, size);
				Color warnaKotak = GetKotakColor(kotak);

				_motifItems.Add(new MotifItem
				{
					Shape = kotak,
					TampilPadaDetik = delay,
					Warna = warnaKotak
				});

				bool putih = (i + j) % 2 == 0;
				if (putih)
				{
					var bunga = BuatMotifBungaKlasik(x + size / 2, y + size / 2, 4);
					foreach (var b in bunga)
					{
						Color warnaBunga = (i % 2 == 0) // baris genap → warna berputar
							? warnaBungaGenap[bungaIndex % warnaBungaGenap.Length]
							: warnaBungaGanjil; // baris ganjil → kuning

						_motifItems.Add(new MotifItem
						{
							Shape = b,
							TampilPadaDetik = delay + 0.03f,
							Warna = warnaBunga
						});
					}

					bungaIndex++;
				}

				delay += delayPerKotak;
			}
		}

		_totalDurasi = delay;
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
