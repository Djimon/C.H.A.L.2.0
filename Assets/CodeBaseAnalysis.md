# Codebase-Analyse & Dokumentations-Vorarbeit

## Kontext & Zielsetzung

Ziel dieser Aufgabe ist es **nicht**, eine vollständige Enddokumentation zu schreiben,  
sondern eine **belastbare Wissensbasis** zu schaffen, aus der später eine saubere,
verlinkte technische Doku entstehen kann.

Der Fokus liegt ausdrücklich auf:
- Systemverständnis
- Beziehungen zwischen Klassen & Systemen
- Daten- und Kontrollflüssen
- nachvollziehbaren Notizen statt „alles im Kopf behalten“

Diese Vorarbeit ist notwendig, da automatisierte (KI-basierte) Dokumentation
zwar einzelne Klassen gut beschreiben kann, aber **systemische Zusammenhänge,
Referenzen und Abhängigkeiten nicht zuverlässig auflöst**.

---

## Arbeitsprinzipien

- **Notizen sind Pflicht**, Kontext ist zu groß für Gedächtnisarbeit
- Lieber **unvollständig aber korrekt** als vollständig geraten
- Fokus auf **Knoten & Flüsse**, nicht auf jede einzelne Klasse
- Annahmen immer kenntlich machen (Hypothesen ≠ Fakten)
- Ziel: Inhalte müssen später **verlinkbar & referenzierbar** sein

---

## Empfohlenes Notiz-Setup

Lege einen Ordner an, z. B.:

```unix
/_notes/
├─ 00_README.md
├─ 01_SystemMap.md
├─ 02_RelationshipMap.md
├─ 03_ReadingPath.md
├─ 04_Glossary.md
├─ 05_OpenQuestions.md
├─ 06_KeyFlows.md
├─ 07_ConventionsAndPatterns.md
└─ systems/
├─ Inventory.md
├─ Loot.md
└─ ...
```

Markdown reicht völlig aus – kein Tool-Zwang.

---

## Phase 1 – Orientierung & Systemverständnis

### Ziel von Phase 1
Ein **stabiles mentales Modell** der Codebase:
- Welche Systeme gibt es?
- Wo sind Einstiegspunkte?
- Wer hängt wovon ab?
- Wo liegen die Komplexitäts-Hotspots?

### 1) Entry Points identifizieren
- Runtime-Startpunkte
- Bootstrap / Initialisierung
- zentrale Manager, Services oder Facades
- Composition Root / DI / Singletons

Notieren:
- Datei
- Verantwortung
- was von dort aus erreicht wird

---

### 2) Subsysteme erfassen (High-Level)

Identifiziere grobe Systeme (auch wenn sie technisch verstreut liegen), z. B.:
- Inventory
- Loot
- Crafting
- Stats / Progression
- Save / Load
- UI / Presentation
- Rules / Config
- Eventing / Signals

Für jedes System eine kurze Notiz:

**System-Template**
- Purpose (1–2 Sätze)
- Einstiegspunkte
- State Ownership
- wichtigste Abhängigkeiten
- bekannte Hotspots

→ Ergebnis: `01_SystemMap.md`

---

### 3) Beziehungs- & Referenz-Map aufbauen

Nicht nur *dass* Klassen zusammenhängen, sondern **wie**.

Beziehungstypen:
- `calls`
- `owns`
- `reads`
- `writes`
- `creates`
- `emits`
- `subscribes`
- `config-driven`

Beispiel:
- `LootService -> calls -> LootRulesService`
- `GameManager -> owns -> StatsService`
- `StatsService -> emits -> OnMapCompleted`

→ Ergebnis: `02_RelationshipMap.md`

---

### 4) Lesepfad definieren

Ziel: Eine sinnvolle Reihenfolge, wie man sich die Codebase erschließt.

Typische Reihenfolge:
1. Bootstrap / Game Loop
2. Globaler State
3. Eventing
4. Domain-Services
5. Rules / Config
6. UI → Domain

→ Ergebnis: `03_ReadingPath.md`

---

## Phase 2 – Vertiefung & Ablaufketten

### Ziel von Phase 2
Die **entscheidenden End-to-End-Zusammenhänge** explizit machen,
damit Referenzen später sauber auflösbar sind.

---

### 5) Key Flows dokumentieren

Wähle 3–5 zentrale Abläufe, z. B.:
- Enemy Kill → Loot Drop
- Map Completed → Progression
- UI Action → Domain Change → Save
- Item Crafting
- Save / Load

**Flow-Template**
- Trigger
- Schrittfolge (1..N, mit Klassen)
- Datenobjekte
- Ergebnis / Side Effects
- Varianten / Edge Cases

→ Ergebnis: `06_KeyFlows.md`

---

### 6) Schlüsselklassen vertiefen

Nicht jede Klasse dokumentieren – nur **Knotenklassen**.

**Klassen-Template**
- Rolle / Verantwortung
- Lebenszyklus (wer erzeugt?)
- wichtigste Methoden
- direkte Kollaborateure
- betroffene Daten
- Side Effects
- Annahmen / Invariants
- Risiken / Missverständnisse

---

### 7) Glossar & offene Fragen pflegen

**Glossar**
- Projektspezifische Begriffe
- Abkürzungen
- implizite Konzepte

**Open Questions**
- präzise formuliert
- idealerweise mit Fundstelle
- keine allgemeinen „Versteh ich nicht“-Notizen

→ Ergebnisse:
- `04_Glossary.md`
- `05_OpenQuestions.md`

---

## Entscheidungs- & Hypothesen-Log (wichtig)

Wenn du interpretierst:
- Annahme klar markieren
- Quelle nennen
- Sicherheit einschätzen (hoch/mittel/niedrig)

Das spart später massiv Zeit bei Validierung.

---

## Qualitätskriterien

Die Ergebnisse sind gut, wenn:
- Beziehungen typisiert sind (nicht nur „nutzt“)
- Aussagen belegbar sind (Datei / Klasse)
- Flows nachvollziehbar sind
- offene Punkte klar benannt sind
- jemand Fremdes damit sinnvoll weiterlesen könnte

---

## Abschluss & Übergabe

Bitte in `00_README.md` kurz festhalten:
- wichtigste Erkenntnisse
- wichtigste Flows
- größte Unsicherheiten
- empfohlener nächster Schritt für die eigentliche Doku

---

Dein Mehrwert ist **Zusammenhang & Struktur**.
