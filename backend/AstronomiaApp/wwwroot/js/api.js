/* api.js — utilidades globales para llamadas AJAX */

const API_BASE_URL = '';

async function apiFetch(path, options = {}) {
  const r = await fetch(API_BASE_URL + path, options);
  if (!r.ok) {
    const err = await r.json().catch(() => ({ mensaje: r.statusText }));
    throw new Error(err.mensaje || 'Error en la API');
  }
  return r.json();
}

// Historial de navegación (Pila — librería JS nativa: Array como pila)
const historialPila = [];

function pushHistorial(id, nombre) {
  if (historialPila.length === 0 || historialPila[historialPila.length - 1].id !== id) {
    historialPila.push({ id, nombre });
    if (historialPila.length > 20) historialPila.shift();
  }
}

function popHistorial() {
  return historialPila.pop();
}
