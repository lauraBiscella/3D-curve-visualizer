# Teoria

## Curve parametriche nello spazio

Formalmente, una curva parametrica nello spazio è una funzione

$$P:I⊂\mathbb{R}→\mathbb{R}^3$$

della forma

$$P(t)=(x(t),y(t),z(t))$$

dove $x(t),y(t),z(t)$ sono funzioni reali del parametro 
$t$ definito nell'intervallo reale $I$. 

### Regolarità
Una curva è detta regolare se la sua derivata prima, cioè il vettore tangente, non si annulla mai:

$$\dot{P}(t)≠0\ \ \ \ \ \ \ ∀t∈I$$

Questo significa che per ogni punto della curve passa una retta tangente e la sua direzione è ben definita. 

Dal punto di vista geometrico potremmo dire che la curva non presenta delle cuspidi.

### Forte regolarità
Una curva è detta fortemente regolare se la sua derivata prima e seconda sono linearmente indipendenti. 

$$\dot{P}(t) \times \ddot{P}(t) \ne 0$$

Questo significa che in ogni punto della curva è sempre definito univocamente un piano osculatore che contiene la retta tangente nel punto. Esso è il piano che meglio approssima la curva localmente. 

Quindi, da un punto di vista geometrico potremmo dire che la curva non è localmente rappresentabile come una retta.

### Frame di Frenet
Il frame di Frenet è un sistema di riferimento che ha l'origine nel punto generico della curva e scorre lungo la curva insieme al parametro t. 
Esso è composto dai vettori
+ Tangente: dato dalla derivata prima, e ci da la direzione del moto
+ Normale: dato dalla derivata seconda, e ci da la direzione della curvatura
+ Binormale: dato dalla derivata terza, perpendicolare al piano osculatore identificato dagli altri due vettori

### Curvatura

La curvatura è una funzione scalare definita per ogni punto della curva che misura quanto la curva devia localmente da una linea retta. 

Essa è definibile con questa equazione:

$$C(t) = \frac{||\dot{P}(t) \times \ddot{P}(t)||}{||\dot{P}(t)||^3}$$

La curvatura è intepretabile come quanto velocemente ruota la tangente.

#### Properietà
+ Sempre positiva
+ Se $C = 0$ allora la curva è localmente rettilinea. Per esempio in corrispondenza di un flesso per il cambio di concavità della curva, data da un cambio di direzione del poligono di controllo.
+ Minimi: la curva è assimilabile con una retta, cioè la curva passa dal punto avvicinandosi molto alla retta tangente.
+ Massimi: la curva presenta un gomito quasi una cuspide, cioè la curva passa al punto allontanandosi molto dalla retta tangente.
+ Non hai mai cuspidi agli incollamenti in una Spline poichè gli incollamenti di tipo $C^2$ garantiscono continuità di tangenza e curvatura, però in corrispondenza dei nodi puoi avere massimi locali o variazioni di concavità. (punti di controllo esterni, poligono cambia direzione)

### Torsione

La torsione è una funziona scalare definita per ogni punto della curva che misura quanto la curva si allontana dal piano osculatore, cioè quanto esso ruota nello spazio.

Essa è definita per curve fortemente regolari di classe $C^2$, con questa equazione:

$$\tau(t) = \frac{(\dot{P}(t) \times \ddot{P}(t))\times \dddot{P}(t)}{||\dot{P}(t) \times \ddot{P}(t)||^2}$$

#### Proprietà
+ Siccome $\dot{P}(t)$ e $\ddot{P}(t)$ sono sempre indipendenti, ma l'indipendenza tra questi due vettori e $\dddot{P}(t)$ non è garantita la funzione può essere identicamente zero.
+ Se $\tau = 0$ il supporto della curva è contenuto in un piano e il piano è quello osculatore. Per questo si definisce solo per curve nello spazio.
+ Se $\tau$ e $C$ sono costanti allora la curva è un'elica cilindrica
+ Se $\tau = 0$ e $C$ è costante allora la curva è una circonferenza.
+ Più la torsione è grande più il piano osculatore si muove rapidamente, quindi la torsione può essere interpretata come: quanto velocemente la curva si discosta dall'essere una curva piana.
+ Il segno positivo o negativo della torsione dipende dal verso in cui viene percorso il supporto della curva e denota se il piano osculatore sta ruotando in un verso o nell'altro, cioè se la curva si trova nel semispazio superiore o inferiore al piano osculatore. 
+ Massimi e minimi rappresentano forti twist della curva.

## Piano osculatore
TO DO

## Teorema di rigidità
Una curva che sia definita su un intervallo chiuso e limitato nello spazio è determinata unicamente, a meno di movimenti rigidi, dalle funzioni curvatura e torsione. 
Definendo quindi dei valori di curvatura e torsione non è possibile deformare la curva nello spazio in modo continuo senza alterarle.

Questo teorema è ciò che denota l'importanta di queste due proprietà della curva e quanto la loro analisi da informazioni essenziali sull'andamento geometrico di una curva.

## Curve di Bezier nello spazio

Le curve di Bézier sono curve parametriche utilizzate nella grafica computerizzata e nel CAD, grazie alla loro semplicità analitica polinomiale.

Sono definite a partire da un insieme di punti nello spazio detti punti di controllo.

### Punti di controllo e poligono di controllo

Siano dati $n+1$ punti

$$P_0,P_1,…,P_n \in \mathbb{R}^3$$

questi punti costituiscono i **punti di controllo**.

Unendo consecutivamente tali punti con dei segmenti si ottiene il **poligono di controllo**.

La curva di Bézier di grado $n$ è definita come:

$$B(t)=\sum_{i=0}^nB_{i,n}(t)P_i$$

con $t \in [0,1]$ e dove $B_{i,n}$ sono i **polinomi di Bernstein**.

### Polinomi di Bernstein
I polinomi di Bernstein sono definiti come:

$$B_{i,n}(t)=\binom{n}{i}t^i(1−t)^{n−i}$$

Essi possiedono proprietà importanti:

+ Positività su [0,1]
+ Partizione dell'unità: $\sum_{i=0}^nB_{i,n}(t)=1$

Grazie a queste proprietà la curva resta sempre all'interno dell'involucro convesso dei punti di controllo, generato dal poligono di controllo.

### Curvatura e torsione
Essendo una curva parametrica polinomiale nello spazio, anche per le curve di Bézier si possono calcolare curvatura e torsione. 

Queste grandezze dipendono dai punti di controllo e descrivono il comportamento locale della curva.

## Curve B-Spline nello spazio 

Le B-spline generalizzano le curve di Bézier permettendo una maggiore flessibilità e controllo locale della forma.

### Punti di De Boor e poligono di De Boor

Sia dato un insieme di punti

$$P_0,P_1,…,P_n$$

detti punti di De Boor (o punti di controllo della B-spline).

La spezzata che li unisce costituisce il poligono di De Boor.

A differenza delle curve di Bézier, ogni punto di controllo influenza solo una porzione limitata della curva.

### Vettore dei nodi

La definizione di una B-spline richiede anche un vettore dei nodi

$$t_0,t_1,…,t_m$$

che è una sequenza non decrescente di numeri reali.

I nodi suddividono il dominio del parametro in segmenti polinomiali, ogni segmento rappresenta il dominio di una curva di Bezier. Infatti le curve B-spline sono il risultato di $l$ incollamenti di curve di Bezier.

### Definizione della curva

Una curva B-spline di grado 
$p$ è definita come:

$$C(t)=\sum_{i=0}^nP_iN_{i,p}(t)$$

dove $N_{i,p}(t)$ sono le funzioni base B-spline definite ricorsivamente.

### Incollamenti (continuità)

Il tipo di collegamento tra i nodi della B-Spline determina il grado di continuità:

+ Continuità $C^0 \rightarrow$ continuità della posizione
+ Continuità $C^1 \rightarrow$
continuità della tangente
+ Continuità $C^2 \rightarrow$ continuità della curvatura

Questo fenomeno è chiamato incollamento nei nodi.

**Per il tipo di B-Spline presente dell'applicazione:** (quindi di grado 3 con incollamenti di 4 curve di Bezier) l'incollamento utlizzato è quello di tipo $C^2$.

### Curvatura e torsione

Anche per le B-spline, essendo curve parametriche nello spazio, curvatura e torsione sono definite tramite le derivate.

La struttura a tratti della B-spline implica che queste quantità siano polinomiali su ciascun intervallo tra nodi.

### Formule passaggio da beziere a de boor
TO DO