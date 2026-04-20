# Teoria

## Curve parametriche nello spazio

Una curva parametrica nello spazio è una funzione

$$P:I⊂\mathbb{R}→\mathbb{E}^3$$

definita nella forma

$$P(t)=(x(t),y(t),z(t)),$$

dove $x(t),y(t),z(t)$ sono funzioni reali del parametro 
$t \in I$. 

### Regolarità
Una curva si dice regolare se il suo vettore tangente (derivata prima), non si annulla mai:

$$\dot{P}(t)≠0\ \ \ \ \ \ \ ∀t∈I.$$

Ciò garantisce che in ogni punto della curva sia ben definita una **retta tangente** e la sua direzione.

Dal punto di vista geometrico una curva regolare non presenta cuspidi.

### Forte regolarità
Una curva è detta fortemente regolare se la sua derivata prima e seconda sono linearmente indipendenti: 

$$\dot{P}(t) \times \ddot{P}(t) \ne 0.$$

Questa condizione implica che in ogni punto della curva è definito univocamente un **piano osculatore**, ossia il piano che contiene la retta tangente e che meglio approssima la curva localmente:

$$\pi = (P(t), <\dot{P}(t), \ddot{P}(t)>).$$

Geometricamente, la curva non è localmente assimilabile a una retta.

### Frame di Frenet
Il frame di Frenet è un sistema di riferimento mobile lungo la curva, con origine nel punto generico $P(t)$. 
Esso è composto da tre vettori ortogonali:
+ Tangente: dato dalla derivata prima, e ci da la direzione del moto
$$T=\frac{\dot{P}}{||\dot{P}||}.$$
+ Normale: dato dalla derivata seconda, e ci da la direzione della curvatura
$$N=\frac{\dot{T}}{||\dot{T}||}.$$
+ Binormale: perpendicolare al piano osculatore identificato dagli altri due vettori
$$B = T \times N.$$
Il piano osculatore può quindi essere definito anche come:
$$\pi = (P(t), <T, N>).$$

### Curvatura

La curvatura è una funzione scalare definita per ogni punto della curva in questo modo: 

$$C(t) = \frac{||\dot{P}(t) \times \ddot{P}(t)||}{||\dot{P}(t)||^3},$$

e misura quanto la curva devia localmente da una linea retta. 

Essa rappresenta la velocità di rotazione del vettore tangente.

### Torsione

La torsione è una funzione scalare definita per ogni punto della curva che misura quanto la curva si allontana dal piano osculatore, ovvero quanto veloemente tale piano ruota nello spazio

Essa è definita per curve fortemente regolari di classe $C^2$, con questa equazione:

$$\tau(t) = \frac{(\dot{P}(t) \times \ddot{P}(t))\times \dddot{P}(t)}{||\dot{P}(t) \times \ddot{P}(t)||^2}.$$

## Curve di Bezier nello spazio

Le curve di Bézier sono curve parametriche polinomiali utilizzate nella grafica digitale e nel CAD.

### Punti di controllo e poligono di controllo

Sia data una sequenza di $n+1$ punti

$$\{P_0,P_1,…,P_n\} \in \mathbb{R}^3,$$

essi costituiscono i **punti di controllo** che definiscono univocamente la curva di Bezier, mentre la spezzata che li unisce è il **poligono di controllo**.

La curva di Bézier di grado $n$ si definisce quindi come:

$$B(t)=\sum_{i=0}^nB_{i}^n(t)P_i$$

con $t \in [0,1]$ e dove $B_{i}^n$ sono i **polinomi di Bernstein**.

### Polinomi di Bernstein
I polinomi di Bernstein sono definiti come:

$$B_{i,n}(t)=\binom{n}{i}t^i(1−t)^{n−i}.$$

Essi possiedono proprietà importanti:

+ Positività su $[0,1]$
+ Partizione dell'unità: $\sum_{i=0}^nB_{i,n}(t)=1$

Da queste proprietà segue che la curva è contenuta nell'involucro convesso dei punti di controllo, generato dal poligono di controllo. Per questo posso controllare l'andamento della curva spostando i punti di controllo.

### Curvatura e torsione
Essendo curve parametriche, anche per le Bézier si possono calcolare curvatura e torsione, che dipendono dai punti di controllo e descrivono il comportamento locale della curva.

### Incollamenti

Le curve di Bezier possono essere incollate seguendo alcune condizioni di incollamento definite sui punti di controllo, che determinano il grado di continuità con cui le varie curve di Bezier sono connesse in corrispondenza di alcuni punti.

Due condizioni principali sono:

+ Si possono incollare solo curve dello stesso grado,
+ Le curve incollate sono fortemente regolari.

Le curve di Bézier sono definite sul'intervallo standard $[0,1]$ ma per poterle incollare serve ridefinire le curve su intervalli generici $[a,b]$, in modo da creare un'unione di intervalli adiacenti. 

Questo si fa tramite il passaggio da una curva ad una equivalente con una funzione $t=t(\tau)$ binuivoca, bicontinua e bidifferenziabile. 

La funzione standard più utilizzata è:

$$t = \frac{\tau-a}{b-a},$$

la quale fa si che quando considero la curva $G(\tau) = P[t(\tau)]$ essa sia la stessa curva $P(t)$ definita sul dominio $[0,1]$ ma definita sul dominio $[a,b]$.

Si possono ora definire le condizioni di incollamento, per cui ciascuna implica la precedente.

Date due curve di Bezier:
+ $P(t)$ definita su intervallo $[a,b]$ con punti di controllo che vanno da $\{P_0,..., P_k\},$ 
+ $Q(\tau)$ definita su intervallo $[b,c]$ con punti di controllo che vanno da $\{Q_0,..., Q_k\},$

abbiamo le sequenti contizioni di incollamento:
+ Incollamento $C^0$: continuità della posizione, non ci sono salti e i supporti sono saldati sul punto
    $$P_k = Q_0,$$

+ Incollamento $C^1$: continuità della tangente, non ci sono spigoli, il vettore tangente varia con continuità:
    $$\dot{P}(b) = \dot{Q}(b).$$
    Svolgendo i calcoli ottenimo 
    $$(c-b)(P_k - P_{k-1})=(b-a)(Q_1-Q_0).$$
    Per semplicità da questo momento chiameremo il punto $Q_0$ come $P_k$ poichè vale $C^0$. Da qui deduciamo che:
    + I due vettori $P_k - P_{k-1}$ e $Q_1 - P_k$ siano dipendenti e quindi i tre punti $P_{k-1}, P_k, Q_1$ sono allineati, quindi la retta tangente varia con continuità del puntoe quindi la retta tangente varia con continuità.
    + Esiste una relazione precisa tra le lunghezze dei segmenti $P_{k-1} P_k$ e $P_kQ_1$ determinata dalla posizione dei valori $a,b,c$ sull'asse dei numeri reali, per cui deve quindi valere la formula:
        $$(a,b,c) = (P_{k-1}, P_k, Q_1).$$
        Quindi il vettore tangente varia con continuità.
    
        Definiamo il **rapporto semplice** di 3 punti $a, b, c$ allineati come:
    $$(a, b, c) = \pm \frac{ac}{bc}.$$

+ Incollamento $C^2$: continuità della curvatura, la transizione è fluida cioè la curvatura non ha salti. Il vettore derivata seconda varia con continuità e quindi geometricamente il piano osculatore varia con continuità e la curvatura varia con continuità:

    $$\ddot{P}(b)=\ddot{Q}(b).$$

    Possiamo dire che:
    + Per condizione di incollamento $C^1$:
    $$(a,b,c) = (P_{k-1}, P_k, Q_1),$$
    + Questa condizione richiede l'introduzione di un punto chiamato punto di De Boor ($D$) ed è il punto di intersezione dei prolungamenti dei segmenti $P_{k-2}P_{k-1}$ e $Q_1Q_2$. Quindi abbiamo come condizione che queste due rette si incontrino in un punto.
    + Inoltre abbiamo anche una condizione sui rapporti semplici:
        $$(P_{k-2}, P_{k-1}, D) = (a,b,c),$$
        $$(D, Q_1, Q_2) = (a,b,c),$$

## Curve B-Spline nello spazio 

Le B-spline generalizzano le curve di Bézier mediante l'incollamento di $l$ curve di Bézier, permettendo una maggiore flessibilità e controllo locale della forma.

Una B-spline cubica può essere vista come una sequenza di curve di Bézier di grado 3 incollate con continuità $C^2$.

### Punti di De Boor e poligono di De Boor

Sia data una sequenza di $n+1$ punti 

$$P_0,P_1,…,P_n \in \mathbb{R}^3,$$

essi costituiscono i **punti di De Boor** (o punti di controllo della B-spline) che definiscono parzialmente la curva B-Spline, mentre la spezzata che li unisce costituisce il **poligono di De Boor**.

A differenza delle curve di Bézier, ogni punto influenza solo una porzione limitata della curva. Nello specifico vengono influenzate al massimo 3 curve di Bezier contemporaneamente, per questo sono più efficienti delle curve di Bezier che invece alzano il grado dei polinomi.

### Vettore dei nodi

Una B-Spline è definita anche da una sequenza non decrescente di valori: 

$$\{n_0,n_1,…,n_m\} \in \mathbb{R},$$

che costituiscono i **nodi** della B-Spline.

I nodi si trovano sul dominio della curva in coincidenza dei punti di incollamento delle curve di Bézier che la compongono, più il punto iniziale del primo intervallo e finale dell'ultimo. 

La distanza tra uno e l'altro è l'intervallo di definizione della singola curva di Bézier e considerati due nodi consecutivi $u_0$ e $u_1$ l'intervallo si definisce come $\Delta_0 = u_1 - u_0$.

I nodi non possono coincidere ne scavallarsi, poiché annullerebbero l'esistenza di una delle Bezier di conseguenza $\Delta_u > 0.$ 

### Definizione della curva

Una curva B-spline di grado 
$p$ è dunque definita come:

$$C(t)=\sum_{i=0}^nP_iN_{i}^p(t),$$

dove $N_{i}^p(t)$ sono le funzioni base B-spline definite ricorsivamente.

### Curvatura e torsione

Anche per le B-spline, essendo curve parametriche nello spazio, curvatura e torsione sono definite tramite le derivate.

La struttura a tratti della B-spline implica che queste quantità siano polinomiali su ciascun intervallo tra nodi.

### Formalismo di Farim: creazione di B-Spline 
Nello specifico useremo come esempio le B-Spline create nell'applicazione cioè di grado 3 con $l=4$.

Per definire una B-Spline ci servono i nodi e i punti di De Boor. Nel nostro caso:
+ Sappiamo che ha 5 nodi, quello iniziale e finale e i 3 nodi legati agli incollamenti
$$\{n_0, ..., n_4\} \in \mathbb{R}$$
+ Sappiamo da questa formula:
    $$\#DeBoor = 2_{inizio} + 2_{fine} + (l-1)$$
    che abbiamo 7 punti di De Boor. I primi due e gli ultimi due coincidenti con i punti di controllo iniziali e finali della prima e ultima curva di Bezier
$$P_0 \equiv D_{-1}$$ $$P_1 \equiv D_0$$ $$P_{11} \equiv D_4$$ $$P_{12} \equiv D_5.$$
e gli altri 3 in corrispondenza degli incollamenti.


Avendo incollato quattro curve di Bezier sappiamo che in totale avremo 13 punti di controllo di Bezier,
$$\{P_0, ..., P_{12}\} \in \mathbb{R^3}$$
poichè sarebbero 4 per poligono meno i tre duplicati agli incollamenti.

Possiamo ricavarne le posizioni e quindi i poligoni di controllo di Bezier tramite queste formule:
$$P_{3p} = \frac{\Delta_p}{\Delta_{p+1}+\Delta_p}P_{3p-1}+\frac{\Delta_{p-1}}{\Delta_{p+1}+\Delta_p}P_{3p+1},$$
$$P_{3p-1} = \frac{\Delta_p}{\Delta}D_{p-1} + \frac{\Delta_{p-2} + \Delta_{p-1}}{\Delta}D_p,$$
$$P_{3p-2} = \frac{\Delta_{p-1}+\Delta_p}{\Delta}D_{p-1}+\frac{\Delta_{p-2}}{\Delta}D_p,$$

dove $\Delta = \Delta_{p-2}+\Delta_{p_1}+\Delta_p.$ 

Sfruttiamo il valore del podice scegliendo quelli in cui è:
+ multiplo di $3$ ($P_3, P_6, P_9$), 
+ multiplo di $3-1$ ($P_2, P_5, P_8$),
+ multiplo di $3+1$ ($P_4, P_7, P_{10}$), che può essere riscritto come $3-2$.

# Teorema di rigidità
Una curva che sia definita su un intervallo chiuso e limitato nello spazio è determinata unicamente, a meno di movimenti rigidi, dalle funzioni curvatura e torsione. 
Definendo quindi dei valori di curvatura e torsione non è possibile deformare la curva nello spazio in modo continuo senza alterarle.

Questo teorema è ciò che denota l'importanta di queste due proprietà della curva e quanto la loro analisi da informazioni essenziali sull'andamento geometrico di una curva.

## Analisi curvatura e torsione
### Curvatura
+ La curvatura è sempre positiva poiché il numeratore è sempre positivo e il denominatore è sempre positivo perché la curva è regolare.
+ Se $C = 0$ allora la curva è localmente rettilinea. Per esempio in corrispondenza di un flesso per il cambio di concavità della curva, data da un cambio di direzione del poligono di controllo. Ma essendo sia la Bezier che la B-Spline curve sono fortemente regolari è impossibile che questo avvenga.
Cuspidi $\dot{P}(t) = 0$ velocità nulla della tangente per regolarità
Flessi $\dot{P} || \ddot{P}$ niente cambi di concavità passando per una retta, la direzione della tangente cambia sempre, tangente ruota sempre (sempre per forte regolarità)
+ Minimi: la curva è assimilabile con una retta, cioè la curva passa dal punto avvicinandosi molto alla retta tangente.
+ Massimi: la curva presenta un gomito quasi una cuspide, cioè la curva passa dal punto allontanandosi molto dalla retta tangente. La velocità della retta tangente è massima.
+ Non hai mai cuspidi agli incollamenti in una Spline poichè gli incollamenti sono di tipo $C^2$ e quindi garantiscono continuità di tangenza e curvatura, però in corrispondenza dei nodi puoi avere massimi locali o variazioni di concavità. (punti di controllo esterni, poligono cambia direzione)

### Proprietà torsione
+ Siccome $\dot{P}(t)$ e $\ddot{P}(t)$ sono sempre indipendenti, ma l'indipendenza tra questi due vettori e $\dddot{P}(t)$ non è garantita la funzione può essere identicamente zero.
+ Se $\tau = 0$ il supporto della curva è contenuto in un piano e il piano è quello osculatore. Per questo si definisce solo per curve nello spazio.
+ Se $\tau$ e $C$ sono costanti allora la curva è un'elica cilindrica
+ Se $\tau = 0$ e $C$ è costante allora la curva è una circonferenza.
+ Più la torsione è grande più il piano osculatore si muove rapidamente, quindi la torsione può essere interpretata come: quanto velocemente la curva si discosta dall'essere una curva piana.
+ Il segno positivo o negativo della torsione dipende dal verso in cui viene percorso il supporto della curva e denota se il piano osculatore sta ruotando in un verso o nell'altro, cioè se la curva si trova nel semispazio superiore o inferiore al piano osculatore, questo è chiaramente visibile dal verso del vettore binormale del frame di frenet, sempre permendicolare al piano osculatore. 
+ Massimi e minimi rappresentano forti twist della curva.

I due grafici in figura mostrano rispettivamente una curva con delle belle proprietà e una con delle brutte propietà geometriche, questo dipende dall'alta o bassa variazione repetina dei valori di curvatura e torsione. Così possiamo analizzare direttamente la bellezza di una curva.