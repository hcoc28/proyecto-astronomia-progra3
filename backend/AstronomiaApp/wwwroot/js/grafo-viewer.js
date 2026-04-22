/* grafo-viewer.js — Visualización del grafo con Canvas 2D */

(function () {
  const canvas = document.getElementById('grafo-canvas');
  if (!canvas) return;

  const ctx = canvas.getContext('2d');
  const data = JSON.parse(document.getElementById('grafo-data').textContent);

  const COLORES_TIPO = {
    'Estrella':    '#f0c040',
    'Planeta':     '#4a90d9',
    'Exoplaneta':  '#7ed56f',
    'Galaxia':     '#c084fc',
    'Satélite':    '#94a3b8',
    'default':     '#64748b'
  };

  // Estado del layout
  let nodos = {};
  let offset = { x: 0, y: 0 };
  let scale = 1;
  let dragging = false;
  let lastMouse = null;
  let hoveredId = null;

  // Inicializar posiciones aleatorias para los nodos
  function inicializar() {
    const W = canvas.offsetWidth || 900;
    const H = 500;
    data.nodos.forEach(n => {
      nodos[n.id] = {
        ...n,
        x: 80 + Math.random() * (W - 160),
        y: 80 + Math.random() * (H - 160),
        vx: 0,
        vy: 0
      };
    });
  }

  // Simulación de fuerzas simple (force-directed)
  function simularPaso() {
    const ids = Object.keys(nodos);

    // Repulsión entre nodos
    for (let i = 0; i < ids.length; i++) {
      for (let j = i + 1; j < ids.length; j++) {
        const a = nodos[ids[i]];
        const b = nodos[ids[j]];
        const dx = b.x - a.x;
        const dy = b.y - a.y;
        const dist = Math.sqrt(dx * dx + dy * dy) || 1;
        const fuerza = 3000 / (dist * dist);
        const fx = (dx / dist) * fuerza;
        const fy = (dy / dist) * fuerza;
        a.vx -= fx; a.vy -= fy;
        b.vx += fx; b.vy += fy;
      }
    }

    // Atracción por aristas
    data.aristas.forEach(a => {
      const o = nodos[a.origen];
      const d = nodos[a.destino];
      if (!o || !d) return;
      const dx = d.x - o.x;
      const dy = d.y - o.y;
      const dist = Math.sqrt(dx * dx + dy * dy) || 1;
      const fuerza = (dist - 120) * 0.03;
      const fx = (dx / dist) * fuerza;
      const fy = (dy / dist) * fuerza;
      o.vx += fx; o.vy += fy;
      d.vx -= fx; d.vy -= fy;
    });

    // Fuerza central (evita que se dispersen)
    const W = canvas.offsetWidth || 900;
    const H = 500;
    ids.forEach(id => {
      const n = nodos[id];
      n.vx += (W / 2 - n.x) * 0.001;
      n.vy += (H / 2 - n.y) * 0.001;
    });

    // Aplicar velocidades con amortiguación
    ids.forEach(id => {
      const n = nodos[id];
      n.vx *= 0.85;
      n.vy *= 0.85;
      n.x += n.vx;
      n.y += n.vy;
    });
  }

  function dibujar() {
    const W = canvas.width;
    const H = canvas.height;
    ctx.clearRect(0, 0, W, H);

    ctx.save();
    ctx.translate(offset.x, offset.y);
    ctx.scale(scale, scale);

    // Aristas
    data.aristas.forEach(a => {
      const o = nodos[a.origen];
      const d = nodos[a.destino];
      if (!o || !d) return;
      ctx.beginPath();
      ctx.moveTo(o.x, o.y);
      ctx.lineTo(d.x, d.y);
      ctx.strokeStyle = 'rgba(100,120,180,0.4)';
      ctx.lineWidth = 1 / scale;
      ctx.stroke();
    });

    // Nodos
    Object.values(nodos).forEach(n => {
      const color = COLORES_TIPO[n.tipo] || COLORES_TIPO['default'];
      const radio = hoveredId === n.id ? 10 : 7;

      ctx.beginPath();
      ctx.arc(n.x, n.y, radio / scale, 0, Math.PI * 2);
      ctx.fillStyle = color;
      ctx.fill();
      ctx.strokeStyle = '#fff';
      ctx.lineWidth = (hoveredId === n.id ? 2 : 1) / scale;
      ctx.stroke();

      // Etiqueta
      if (scale > 0.5) {
        ctx.fillStyle = '#e0e0f0';
        ctx.font = `${11 / scale}px Segoe UI, sans-serif`;
        ctx.fillText(n.nombre, n.x + 9 / scale, n.y + 4 / scale);
      }
    });

    ctx.restore();
  }

  // Detección de hover
  canvas.addEventListener('mousemove', e => {
    const rect = canvas.getBoundingClientRect();
    const mx = (e.clientX - rect.left - offset.x) / scale;
    const my = (e.clientY - rect.top - offset.y) / scale;

    if (dragging && lastMouse) {
      offset.x += e.clientX - lastMouse.x;
      offset.y += e.clientY - lastMouse.y;
      lastMouse = { x: e.clientX, y: e.clientY };
      return;
    }

    hoveredId = null;
    Object.values(nodos).forEach(n => {
      const dx = n.x - mx;
      const dy = n.y - my;
      if (Math.sqrt(dx * dx + dy * dy) < 12 / scale) hoveredId = n.id;
    });
    canvas.style.cursor = hoveredId ? 'pointer' : (dragging ? 'grabbing' : 'grab');
  });

  // Click → navegar al detalle
  canvas.addEventListener('click', e => {
    if (hoveredId) {
      window.location.href = `/Objetos/Detalle/${hoveredId}`;
    }
  });

  // Pan con drag
  canvas.addEventListener('mousedown', e => {
    dragging = true;
    lastMouse = { x: e.clientX, y: e.clientY };
    canvas.style.cursor = 'grabbing';
  });

  canvas.addEventListener('mouseup', () => { dragging = false; lastMouse = null; });
  canvas.addEventListener('mouseleave', () => { dragging = false; lastMouse = null; });

  // Zoom con rueda
  canvas.addEventListener('wheel', e => {
    e.preventDefault();
    const factor = e.deltaY < 0 ? 1.1 : 0.9;
    scale = Math.max(0.2, Math.min(5, scale * factor));
  }, { passive: false });

  // Ajustar tamaño del canvas al contenedor
  function ajustarTamanio() {
    canvas.width = canvas.offsetWidth;
    canvas.height = 500;
  }

  // Loop principal
  let tick = 0;
  function loop() {
    if (tick < 200) { simularPaso(); tick++; }
    dibujar();
    requestAnimationFrame(loop);
  }

  ajustarTamanio();
  inicializar();
  loop();
})();
