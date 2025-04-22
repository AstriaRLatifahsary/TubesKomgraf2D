==============================
🎨 Readme.txt - Proyek Budaya Bali Interaktif
==============================

📌 Nama Program:
Proyek Visualisasi dan Animasi Budaya Bali Interaktif
(Meliputi: Pura, Bunga Kamboja, Motif Endek, Sate Lilit, Pola Poleng)

👤 Penulis Program:
Astria Rizka Latifahsary  
Menggunakan Godot Engine dan Bahasa Pemrograman C#

📝 Deskripsi:
Proyek ini adalah serangkaian karya visual dan animasi yang merepresentasikan kekayaan budaya Bali secara digital dan interaktif. Setiap karya dibuat dengan teknik geometris dan efek animasi dinamis, serta dapat dikendalikan oleh pengguna melalui keyboard. Elemen-elemen visual seperti pura, bunga kamboja, motif kain tradisional Endek dan Poleng, hingga makanan khas seperti sate lilit divisualisasikan dengan pendekatan artistik dan terstruktur.

📅 Jadwal Planning dan Progress:

🔹1
- Riset visual budaya Bali
- Menentukan bentuk geometris untuk tiap elemen

🔹2
- Implementasi bentuk statis:
  - Pura
  - Bunga kamboja
  - Motif Endek
  - Sate lilit
  - Pola poleng checkerboard

🔹3
- Menambahkan animasi:
  - Efek nafas pura & pulau
  - Bunga berputar dan berdetak
  - Motif Endek bergerak seperti terkena angin
  - Animasi sate lilit naik-turun
  - Pola poleng muncul bertahap + gelombang

🔹4
- Pewarnaan visual dan efek warna dinamis
- Penyesuaian transparansi dan gradasi

🔹5
- Menambahkan interaksi keyboard untuk masing-masing motif
- Mengatur logika kontrol (pause, skala, arah, warna, posisi)

🔹6 (Finalisasi)
- Pengujian interaksi dan animasi

🎯 Output Akhir:
Kumpulan karya visual budaya Bali yang dapat ditampilkan secara real-time, interaktif, dan estetis, menggunakan Godot Engine berbasis C#.

---

## 🎨 Pura Visualisasi dan Animasi dengan Godot Engine

Visualisasi programatik sebuah **pura** (tempat ibadah umat Hindu) yang digambar dan dianimasikan menggunakan **Godot Engine** dan bahasa **C#**. Bangunan pura terdiri dari bentuk-bentuk geometri dasar seperti persegi panjang, trapesium bertingkat, dan segitiga, serta sebuah **pulau penopang berbentuk setengah elips** di bawahnya. Proyek ini juga mendukung **animasi efek nafas** dan **interaktivitas** melalui input keyboard.

## 🎨 Proyek Bunga Kamboja - Godot Engine (C#)

Visualisasi **bunga kamboja** yang dibentuk menggunakan **Godot Engine** dengan bahasa pemrograman **C#**. Proyek ini dibagi menjadi beberapa tahap: mulai dari menggambar bentuk dasar bunga, menambahkan animasi, pewarnaan dengan efek, hingga menambahkan interaksi pengguna.

## 🎨 Motif Endek Bali - Visualisasi Interaktif di Godot Engine

Representasi digital dari **motif kain tradisional Endek Bali** dalam bentuk visual dinamis yang dibuat menggunakan **Godot Engine** dan **C#**. Proyek ini menampilkan empat komponen utama: _bentuk_, _animasi_, _warna_, dan _interaksi pengguna_ untuk menghadirkan pengalaman visual yang artistik dan interaktif.

## 🎨 Proyek Visualisasi dan Animasi Sate Lilit

Representasi visual dan animasi sate lilit, makanan khas Bali. Terdapat empat tahap pengembangan yaitu **Bentuk**, **Animasi**, **Warna**, dan **Interaksi**.

## 🎨 Proyek Visualisasi Pola Poleng Bali dengan Motif Bunga Klasik

Visualisasi menggunakan **Godot Engine** dan bahasa **C#** untuk menampilkan **pola kain poleng** khas Bali — kain kotak-kotak hitam putih — yang dipadukan dengan **motif bunga klasik bergaya pixel art**. Proyek ini terdiri dari empat tahapan utama:

---

# **Karya 1**

## 🔷 1. Bentuk: Visualisasi Statik Pura

### Deskripsi
Visualisasi statik pura digambar menggunakan metode `DrawLine` dan `DrawPolyline` pada node `Node2D`, yang menggambarkan:
- **Dasar**: Persegi panjang
- **Tingkat**: Trapesium bertingkat
- **Atap**: Segitiga sama kaki
- **Pulau**: Setengah elips sebagai penopang

### Struktur Kode
- `Karya1 : Node2D` — Node utama yang menggambar bentuk pura
- **Metode**:
  - `InisialisasiPura()` — Menentukan posisi dan bentuk bangunan
  - `GeneratePulauCustom()` — Menghasilkan titik-titik setengah elips
  - `DrawPolygonLines()` — Menggambar bentuk dengan garis

## 🌸 2. Bentuk: Visualisasi Bunga Dasar

### Deskripsi
Bunga kamboja digambar secara programatik menggunakan kelopak berbentuk **elips** yang tersusun melingkar. Setiap kelopak dibuat dengan elips dan disusun mengelilingi pusat bunga.

### Fitur
- Menggambar beberapa bunga pada posisi berbeda.
- Kelopak berbentuk elips tersusun radial.
- Menggunakan metode `DrawLine` dan `DrawPolygonLines`.

### Struktur Kode
- `BentukDasar.cs`: Membuat bentuk dasar elips.
- `Bunga.cs`: Menyusun kelopak menjadi satu bunga dan menggambarnya.

## 🧱 3. Bentuk: Visualisasi Motif Endek Dasar

### Deskripsi
Pola dasar yang membentuk motif kain Endek terdiri dari bentuk-bentuk geometris seperti:

- 🔷 **Belah ketupat besar dan kecil**
- ▭ **Persegi panjang dan kotak kecil**

Bentuk-bentuk ini disusun dalam _grid baris dan kolom_ dengan offset pada baris ganjil untuk membentuk **pola zig-zag** khas anyaman tradisional Bali.

### Parameter Grid
- Jumlah Baris: `5`
- Jumlah Kolom: `7`
- Jarak Horizontal (X): `40px`
- Jarak Vertikal (Y): `45px`
- Offset Baris Ganjil: `20px` (setengah jarak X)

### Fungsi Kunci
- `InisialisasiMotifEndek()` — Menyusun bentuk ketupat dan kotak kecil dalam grid.
- `DrawPolygonLines()` — Menggambar outline tiap bentuk geometris secara manual.

### Output Visual
Motif akan tampak seperti susunan anyaman simetris dengan kotak kecil di tengah ketupat besar, dikelilingi bingkai luar.

## 🟦 4. Bentuk: Visualisasi Dasar Sate Lilit

### Sate Lilit

Visualisasi ini menampilkan **Sate Lilit**, makanan khas Bali, dengan menggunakan kombinasi bentuk-bentuk geometris sederhana.

### Struktur Kode

✅ **Property**  
- `BentukDasar _bentukDasar`: Objek untuk menggambar bentuk dasar (persegi panjang dan elips).  
- `List<List<Vector2>> _sateLilitList`: Menyimpan titik-titik bentuk sate lilit.

🚀 **Method**
- `_Ready()`: Inisialisasi objek bentuk dan posisi sate lilit.
- `_Draw()`: Menggambar garis antar titik-titik setiap tusuk sate lilit.
- `DrawPolygonLines()`: Fungsi utilitas menggambar garis tertutup dari titik-titik.
- `SateLilit(...)`: Membuat representasi bentuk sate lilit secara geometris.

🎯 **Hasil Visual**  
Dua set sate lilit simetris kiri dan kanan bawah layar.

## 📐 5. Bentuk: Pola Poleng Dasar

### 📦 Deskripsi
Visualisasi sederhana pola kain poleng berbentuk kotak hitam-putih (checkerboard), ditambah motif bunga klasik di kotak putih.

### 🧩 Struktur
- Grid berukuran 5x6 dengan ukuran tiap kotak 50x50 pixel.
- Kotak putih diberi hiasan bunga klasik pixel 9x9.

### 🛠️ Komponen Kode
- `_motifPolengList`: Menyimpan titik-titik bentuk untuk digambar.
- `BuatPersegi(x, y, sisi)`: Menghasilkan outline persegi.
- `BuatMotifBungaKlasik(cx, cy, spacing)`: Membuat bunga berbasis pixel.
- `_Draw()`: Menggambar semua bentuk putih.

### 🎯 Hasil
Tampilan pola kotak-kotak hitam-putih dengan motif bunga di tengah kotak putih, digambar sebagai outline poligon putih.

---

# **Karya 2**

## 🌬️ 1. Animasi: Efek Nafas Pura dan Pulau

### Deskripsi
Bentuk pura dan pulau dianimasikan dengan transformasi skala yang membuat keduanya tampak **bernafas**, menggunakan fungsi `sin(time)`.

### Fitur
- **Animasi Skala Dinamis** — Memberi kesan hidup dan lembut
- **Redraw Otomatis** melalui `_Process()` dan `_Draw()`

### Struktur Kode
- `_Ready()` — Inisialisasi bentuk
- `_Process(delta)` — Perbarui waktu animasi
- `_Draw()` — Gambar ulang bentuk dengan animasi


## 🎞️ 2. Animasi: Bunga Berputar dan Berdetak

### Deskripsi
Bunga dianimasikan dengan **rotasi** dan **denyut (skala berubah)**. Setiap bunga tampak hidup dengan gerakan yang terus berubah seiring waktu.

### Fitur
- **Rotasi Dinamis**: Bunga berputar berdasarkan waktu.
- **Denyut Halus**: Bunga membesar dan mengecil secara sinusoidal.
- **Transparansi (Alpha)**: Bunga memudar dan muncul kembali dengan kedipan halus.

### Struktur Kode
- `_Process(double delta)`: Memperbarui waktu dan memicu redrawing.
- `DrawWithRotationAndScale()`: Menggambar dengan rotasi dan skala.
- `DrawPolygonLines()`: Menggambar outline poligon.

## 🌊 3. Animasi: Motif Endek

### Deskripsi
Motif Endek dianimasikan untuk menciptakan kesan hidup dan dinamis seperti diterpa angin.

### Gerakan Utama
- 🔼 **Triangle Movement** — Pola diagonal segitiga antar titik motif.
- 🌬️ **Efek Gelombang** — Menggunakan `Mathf.Sin()` untuk menciptakan gerakan vertikal naik-turun seperti efek angin.

### Fungsi Kunci
- `_Process(delta)` — Menghitung pergeseran waktu (`_time`) dan mengubah posisi setiap titik motif.
- `TriangleMovement(t)` — Interpolasi titik secara segitiga.
- `_Draw()` — Menggambar motif di setiap frame berdasarkan posisi terbaru.

### Parameter Animasi
- `amplitudo`: Seberapa tinggi gelombang bergerak.
- `frekuensiGerakan`: Seberapa cepat pola bergelombang.
- `offset`: Posisi awal gerakan segitiga.

### Hasil
Pola motif bergerak secara diagonal dengan efek naik-turun, seolah-olah sedang ditiup angin.

## 4. Animasi: Sate Lilit Animasi

Proyek ini menambahkan animasi ke sate lilit, di mana tiap tusuk sate dan lilitannya bergerak naik-turun dengan efek sinusoidal.

### Fitur:
- Animasi gerakan vertikal sinusoidal.
- Setiap bagian sate bergerak dengan offset bergantian.
- Menggunakan _Process untuk update waktu dan posisi.

### Struktur Kode:
- `SateLilit`: Membuat bentuk sate lilit.
- `_Process()`: Update posisi berdasarkan waktu.
- `_Draw()`: Menggambar sate dengan posisi yang berubah.
- `OffsetPoints()`: Geser titik untuk animasi.

## 5. Animasi: Pola Poleng Muncul Bertahap + Efek Gelombang

### 📦 Deskripsi
Menampilkan motif bunga satu per satu pada kotak putih, lalu menambahkan efek gelombang sinusoidal setelah semua motif muncul.

### 🧩 Struktur
- `MotifItem`: Menyimpan shape dan waktu tampil motif.
- `_Process(delta)`: Menghitung waktu yang berjalan.
- `_Draw()`: Menggambar motif yang sudah waktunya tampil + efek gelombang.

### 🔁 Logika Animasi
- Setiap kotak putih akan menampilkan bunga secara bertahap.
- Setelah semua motif muncul, setiap titik motif bergelombang (gerak naik-turun secara sinusoidal).

### 🎯 Hasil
Animasi progresif dari bunga pixel yang muncul lalu bergerak seperti terkena angin atau gelombang.

---

# **Karya 3**

## 🌈 1. Warna: Pura Pulau Poligon Berwarna

### Deskripsi
Bentuk pura dan pulau tidak hanya digambar dengan garis, namun juga **diwarnai** dengan pengisian warna menggunakan `DrawPolygon`.

### Fitur
- **Gambar Berwarna**: Persegi panjang, trapesium, dan segitiga diberi warna
- **Pulau Berwarna**: Penopang bawah pura berbentuk setengah elips dengan gradasi warna lembut
- **Transformasi Skala dan Warna** — Digunakan bersama untuk membuat efek visual menarik

### Struktur Tambahan
- `DrawPolygon(points, Color color)` — Menggambar isi poligon
- `DrawWithTransform(origin, scale, drawAction)` — Menggambar dengan skala

## 🎨 2. Warna: Pewarnaan Kelopak dan Inti Bunga

### Deskripsi
Setiap bunga memiliki kelopak putih yang memudar secara dinamis, serta lingkaran tengah berwarna kuning. Efek denyut dan transparansi menambah nuansa hidup pada bunga.

### Fitur
- **Kelopak Berwarna Putih**: Diberi efek alpha dinamis.
- **Inti Bunga Berwarna Kuning**: Diberikan efek denyut dan fade.
- **Efek Sinus** untuk animasi kedip dan ukuran.

### Struktur Kode
- `Color kelopak = new Color(1, 1, 1, alpha);`
- `Color inti = new Color(1, 1, 0, alpha);`
- `DrawCircle()` untuk menggambar inti bunga.

## 🎨 3. Warna: Motif Endek

### Deskripsi
Setiap motif memiliki efek warna transparan dan dapat berubah secara dinamis seiring waktu.

### Fitur Warna
- 🌈 **Transparansi Dinamis** — Setiap bentuk digambar dengan `Modulate` transparan.
- 🔄 **Efek Warna Hidup** — Warna motif berubah-ubah mengikuti waktu untuk memberikan kesan hidup.
- 🟪 **Outline Poligon** — Garis luar tiap bentuk menggunakan warna berbeda dari isi.

### Implementasi
- Warna ditentukan secara real-time menggunakan fungsi acak dan waktu (`GD.RandRange()` atau `t`).
- Fungsi `_Draw()` mengatur warna isi dan outline bentuk berdasarkan waktu.

### Efek Visual
- Motif tampak berkedip-kedip atau berubah warna secara lembut, memperkuat kesan artistik dan hidup.

## 4. Warna: Sate Lilit Animasi 

Menambahkan warna untuk meningkatkan visualisasi sate lilit:
- Batang tusuk sate: Hijau
- Lilitan daging: Coklat

### Fitur:
- Warna ditambahkan saat menggambar poligon.
- Perbedaan visual yang jelas antara komponen sate.
- Desain modular untuk pemisahan kode dan warna.

### Struktur Kode:
- `DrawPolygon()`: Menggambar poligon dengan fill warna.
- Parameter warna untuk masing-masing bagian sate.

## 🌈 5. Warna: Pola Poleng dengan Fill Warna dan Efek Gelombang

### 📦 Deskripsi
Menerapkan warna-warni pada kotak dan bunga, disertai efek gelombang waktu nyata (real-time wave).

### 🧩 Struktur
- Setiap elemen memiliki properti `warna` berdasarkan baris dan kolom.
- Efek vertikal sinusoidal diterapkan pada semua titik.
- Kombinasi pola checkerboard dengan rotasi warna pada bunga.

### 🎨 Warna
- Bunga di baris genap: warna rotasi.
- Bunga di baris ganjil: warna tetap.
- Kotak juga memiliki warna pengisi yang berganti-ganti.

### 🎯 Hasil
Visualisasi dinamis dengan warna-warna cerah yang bergerak mengikuti efek gelombang.

---

# **Karya 3**

## 🎮 1. Interaksi: Pura dan Pulau Kontrol Animasi dengan Keyboard

### Deskripsi
Animasi kini bisa **dikendalikan oleh pemain** melalui input keyboard:

### Kontrol
- ⏯️ **P** — Pause/Resume animasi
- ⏩ **Space** — Percepat animasi efek nafas

### Fitur
- **Pause Toggle**: Menjeda dan melanjutkan animasi
- **Frekuensi Animasi Dinamis**: Mengubah kecepatan denyut dengan tombol `Space`

### Struktur Kode Tambahan
- `Input.IsActionJustPressed("pause")` → Toggle animasi
- `Input.IsActionPressed("accelerate")` → Modifikasi frekuensi sinus

## 🎮 2. Interaksi: Bunga Kendali Animasi dengan Keyboard

### Deskripsi
Pengguna dapat berinteraksi dengan bunga menggunakan tombol keyboard untuk mengubah arah rotasi, memperbesar/memperkecil bunga, serta menjeda animasi.

### Kontrol
- ⬆️ **Shift**: Mengubah arah rotasi.
- 🔍 **T**: Memperbesar ukuran bunga.
- 🔽 **K**: Memperkecil ukuran bunga.
- ⏸️ **P**: Menjeda atau melanjutkan animasi.

### Struktur Kode
- `Input.IsActionJustPressed("shift")`: Membalik rotasi.
- `Input.IsActionJustPressed("t")`: Perbesar skala.
- `Input.IsActionJustPressed("k")`: Perkecil skala.
- `Input.IsActionJustPressed("p")`: Toggle jeda animasi.

## 📦 Dependensi

- `BentukDasar.cs`: Fungsi utilitas untuk membuat bentuk elips.
- `Bunga.cs`: Kelas inti bunga, mencakup posisi, rotasi, skala, dan warna.
- `Main.cs` (atau Node utama): Menjalankan animasi, menggambar bunga, dan menangani input.

## 🕹️ 3. Interaksi: Motif Endek

### Deskripsi
Pengguna dapat berinteraksi dengan motif secara langsung menggunakan input keyboard.

### Kontrol Interaksi
- Tekan `W` — Mengubah warna garis luar (_outline_) dari setiap bentuk ke warna acak baru.

### Implementasi
- Input dideteksi menggunakan `_Input(event)` atau logika dalam `_Process`.
- Warna garis disimpan dalam variabel `_polylineColor` dan diperbarui saat `W` ditekan.

### Efek
- Memberikan pengalaman _interaktif_ yang memperkuat eksplorasi pengguna terhadap motif.
- Warna garis motif akan berubah setiap kali tombol ditekan, menghasilkan variasi visual baru.

### Potensi Ekstensi
- Menambahkan kontrol lainnya (seperti slider atau mouse click) untuk mengubah amplitudo atau frekuensi animasi secara langsung.

## 4. Interaksi: Sate Lilit Animasi Fill Warna

Menambahkan interaktivitas ke dalam animasi sate lilit dengan tombol keyboard:
- Tekan **G** untuk mendekatkan sate.
- Tekan **L** untuk menjauhkan sate.

### Fitur:
- Gerakan naik-turun animasi.
- Interaksi keyboard untuk ubah jarak antar sate.
- Visual lebih responsif terhadap input pengguna.

### Struktur Kode:
- `Input()`: Mendeteksi input pengguna.
- `UpdateSateLilit()`: Perbarui posisi sate berdasarkan jarak.
- `_Draw()`: Menggambar sate berdasarkan posisi baru.

## 🎮 5. Interaksi: Pola Poleng dengan Pergerakan Manual via Keyboard

### 📦 Deskripsi
Menambahkan kontrol pengguna pada animasi pola. Pola dapat digerakkan menggunakan keyboard (A, D, U, B).

### 🎮 Kontrol:
- `A`: Gerak ke kiri
- `D`: Gerak ke kanan
- `U`: Gerak ke atas
- `B`: Gerak ke bawah

### 🧩 Struktur
- `_Input()`: Menangkap input tombol dan menggeser posisi pola.
- Motif tetap tampil bertahap + efek gelombang setelah tampil.
- Perubahan posisi objek mempengaruhi semua motif secara serempak.

### 🎯 Hasil
Visual pola yang bisa dikendalikan pengguna sambil tetap mempertahankan animasi dan gelombang.
