// Graph Visualizer using vis.js
// Stores active graph instances
const graphInstances = new Map();

/**
 * Oblicza poziomy węzłów na podstawie struktury grafu
 * @param {Array} nodes - Tablica węzłów
 * @param {Array} edges - Tablica krawędzi
 * @param {Function} getNodeId - Funkcja do pobrania ID węzła
 * @param {Function} getNodeIsStart - Funkcja do sprawdzenia czy węzeł jest startowy
 * @returns {Map} Mapa nodeId -> level
 */
function calculateNodeLevels(nodes, edges, getNodeId, getNodeIsStart) {
    const levels = new Map();
    const nodeIds = new Set(nodes.map(n => getNodeId(n)));
    
    // Węzły startowe zawsze na poziomie 0
    nodes.forEach(node => {
        if (getNodeIsStart(node)) {
            levels.set(getNodeId(node), 0);
        }
    });
    
    // Jeśli nie ma krawędzi, wszystkie węzły bez poziomu ustaw na 1
    if (!edges || edges.length === 0) {
        nodes.forEach(node => {
            const nodeId = getNodeId(node);
            if (!levels.has(nodeId)) {
                levels.set(nodeId, 1);
            }
        });
        return levels;
    }
    
    // Obsługa zarówno camelCase jak i PascalCase dla krawędzi
    const getEdgeFrom = (edge) => edge.from ?? edge.From;
    const getEdgeTo = (edge) => edge.to ?? edge.To;
    
    // Buduj graf odwrotny (kto wskazuje na dany węzeł)
    const incomingEdges = new Map();
    edges.forEach(edge => {
        const toId = getEdgeTo(edge);
        if (!incomingEdges.has(toId)) {
            incomingEdges.set(toId, []);
        }
        incomingEdges.get(toId).push(getEdgeFrom(edge));
    });
    
    // BFS od węzłów startowych
    const queue = [];
    nodes.forEach(node => {
        if (getNodeIsStart(node)) {
            queue.push({ id: getNodeId(node), level: 0 });
        }
    });
    
    // Jeśli nie ma węzłów startowych, użyj węzłów bez krawędzi przychodzących
    if (queue.length === 0) {
        nodes.forEach(node => {
            const nodeId = getNodeId(node);
            if (!incomingEdges.has(nodeId)) {
                queue.push({ id: nodeId, level: 0 });
                levels.set(nodeId, 0);
            }
        });
    }
    
    const visited = new Set();
    
    while (queue.length > 0) {
        const current = queue.shift();
        if (visited.has(current.id)) continue;
        visited.add(current.id);
        levels.set(current.id, current.level);
        
        // Znajdź wszystkie węzły, do których prowadzą krawędzie z tego węzła
        edges.forEach(edge => {
            const fromId = getEdgeFrom(edge);
            const toId = getEdgeTo(edge);
            if (fromId === current.id && !visited.has(toId)) {
                queue.push({ id: toId, level: current.level + 1 });
            }
        });
    }
    
    // Ustaw poziom dla węzłów, które nie zostały odwiedzone
    nodes.forEach(node => {
        const nodeId = getNodeId(node);
        if (!levels.has(nodeId)) {
            // Znajdź minimalny poziom na podstawie krawędzi przychodzących
            let minLevel = 0;
            if (incomingEdges.has(nodeId)) {
                const incoming = incomingEdges.get(nodeId);
                const incomingLevels = incoming
                    .map(fromId => levels.get(fromId))
                    .filter(level => level !== undefined);
                if (incomingLevels.length > 0) {
                    minLevel = Math.max(...incomingLevels) + 1;
                }
            }
            levels.set(nodeId, minLevel);
        }
    });
    
    // UPEWNIJ SIĘ, że wszystkie węzły mają poziom (wymagane przez vis.js)
    nodes.forEach(node => {
        const nodeId = getNodeId(node);
        if (!levels.has(nodeId) || levels.get(nodeId) === undefined) {
            console.warn(`Węzeł ${nodeId} nie ma poziomu, ustawiam 0`);
            levels.set(nodeId, 0);
        }
    });
    
    return levels;
}

/**
 * Dzieli tekst na 3 wiersze dla labeli w grafie
 * @param {string} text - Tekst do podziału
 * @returns {string} Tekst z podziałem na 3 wiersze używając \n
 */
function wrapLabelInThreeLines(text) {
    if (!text) return '';
    
    const words = text.split(' ');
    const totalWords = words.length;
    
    if (totalWords <= 3) {
        // Jeśli jest mało słów, po prostu dodaj \n między nimi
        return words.join('\n');
    }
    
    // Podziel na 3 części
    const wordsPerLine = Math.ceil(totalWords / 3);
    const lines = [];
    
    for (let i = 0; i < totalWords; i += wordsPerLine) {
        lines.push(words.slice(i, i + wordsPerLine).join(' '));
    }
    
    // Upewnij się, że mamy dokładnie 3 wiersze
    while (lines.length < 3) {
        lines.push('');
    }
    
    return lines.slice(0, 3).join('\n');
}

/**
 * Inicjalizuje graf dla danego kontenera
 * @param {string} containerId - ID kontenera HTML
 * @param {Array} nodes - Tablica węzłów grafu
 * @param {Array} edges - Tablica krawędzi grafu
 * @param {object} dotnetRef - Referencja do komponentu Blazor
 */
export function initializeGraph(containerId, nodes, edges, dotnetRef) {
    const container = document.getElementById(containerId);
    if (!container) {
        console.error(`Container ${containerId} not found`);
        return;
    }

    // Upewnij się, że kontener ma wymiary przed inicjalizacją
    const containerRect = container.getBoundingClientRect();
    if (containerRect.height === 0 || containerRect.height < 100) {
        console.warn(`Container ${containerId} has invalid height: ${containerRect.height}, waiting for resize...`);
        // Poczekaj na wymiary kontenera
        const checkDimensions = () => {
            const rect = container.getBoundingClientRect();
            if (rect.height > 100) {
                console.log(`Container ${containerId} now has height: ${rect.height}`);
                // Ustaw jawnie wysokość jeśli nie jest ustawiona
                if (!container.style.height || container.style.height === '') {
                    container.style.height = `${rect.height}px`;
                }
                initializeGraphInternal(containerId, nodes, edges, dotnetRef);
            } else {
                setTimeout(checkDimensions, 50);
            }
        };
        setTimeout(checkDimensions, 100);
        return;
    }

    // Ustaw jawnie wysokość jeśli nie jest ustawiona
    if (!container.style.height || container.style.height === '') {
        container.style.height = `${containerRect.height}px`;
    }

    initializeGraphInternal(containerId, nodes, edges, dotnetRef);
}

function initializeGraphInternal(containerId, nodes, edges, dotnetRef) {
    const container = document.getElementById(containerId);
    if (!container) {
        console.error(`Container ${containerId} not found`);
        return;
    }

    // Logowanie dla debugowania
    console.log('Initializing graph:', { 
        containerId, 
        nodeCount: nodes?.length || 0, 
        edgeCount: edges?.length || 0,
        containerHeight: container.getBoundingClientRect().height
    });
    
    // Sprawdź strukturę pierwszego węzła i krawędzi
    if (nodes && nodes.length > 0) {
        console.log('First node structure:', nodes[0]);
        console.log('Node keys:', Object.keys(nodes[0]));
    }
    if (edges && edges.length > 0) {
        console.log('First edge structure:', edges[0]);
        console.log('Edge keys:', Object.keys(edges[0]));
    }

    // Walidacja danych
    if (!nodes || nodes.length === 0) {
        console.error('No nodes provided');
        return;
    }

    // Przygotuj dane dla vis.js
    // Przechowuj oryginalne dane węzłów dla tooltipów
    // Obsługa zarówno camelCase (id) jak i PascalCase (Id)
    const getNodeId = (node) => node.id ?? node.Id;
    const getNodeName = (node) => node.name ?? node.Name;
    const getNodeTag = (node) => node.tag ?? node.Tag;
    const getNodeIsStart = (node) => node.isStart ?? node.IsStart;
    const getNodeProgress = (node) => node.progress ?? node.Progress;
    const getNodePrefix = (node) => node.prefix ?? node.Prefix;
    const getNodeSystemDescription = (node) => node.systemDescription ?? node.SystemDescription;
    
    const nodeDataMap = new Map();
    const nodeIds = new Set();
    nodes.forEach(node => {
        const nodeId = getNodeId(node);
        nodeDataMap.set(nodeId, node);
        nodeIds.add(nodeId);
    });

    // Obsługa zarówno camelCase jak i PascalCase dla krawędzi
    const getEdgeId = (edge) => edge.id ?? edge.Id;
    const getEdgeFrom = (edge) => edge.from ?? edge.From;
    const getEdgeTo = (edge) => edge.to ?? edge.To;
    
    // Waliduj krawędzie - sprawdź czy wszystkie węzły istnieją
    const validEdges = (edges || []).filter(edge => {
        const fromId = getEdgeFrom(edge);
        const toId = getEdgeTo(edge);
        if (!nodeIds.has(fromId)) {
            console.warn(`Edge ${getEdgeId(edge)} references non-existent node: ${fromId}`);
            return false;
        }
        if (!nodeIds.has(toId)) {
            console.warn(`Edge ${getEdgeId(edge)} references non-existent node: ${toId}`);
            return false;
        }
        return true;
    });

    console.log('Valid edges:', validEdges.length, 'out of', edges?.length || 0);

    // Oblicz poziomy dla węzłów na podstawie krawędzi
    // Vis.js wymaga: wszystkie węzły muszą mieć poziom LUB żaden nie może mieć
    const nodeLevels = calculateNodeLevels(nodes, validEdges, getNodeId, getNodeIsStart);
    
    // Upewnij się, że WSZYSTKIE węzły mają poziom (wymagane przez vis.js)
    const allNodesHaveLevel = nodes.every(node => {
        const nodeId = getNodeId(node);
        return nodeLevels.has(nodeId) && nodeLevels.get(nodeId) !== undefined;
    });
    
    console.log('All nodes have level:', allNodesHaveLevel);
    console.log('Node levels map:', Array.from(nodeLevels.entries()));
    
    // Sprawdź czy wszystkie węzły mają poziom
    nodes.forEach(node => {
        const nodeId = getNodeId(node);
        if (!nodeLevels.has(nodeId) || nodeLevels.get(nodeId) === undefined) {
            console.error(`BŁĄD: Węzeł ${nodeId} (${getNodeName(node)}) nie ma poziomu!`);
            nodeLevels.set(nodeId, 0); // Ustaw domyślny poziom
        }
    });

    // Przygotuj węzły - upewnij się, że wszystkie mają poziom jako LICZBĘ (nie undefined)
    const visNodesData = nodes.map(node => {
        const nodeId = getNodeId(node);
        let level = nodeLevels.get(nodeId);
        
        // Upewnij się, że level jest liczbą
        if (level === undefined || level === null || isNaN(level)) {
            console.error(`BŁĄD: Węzeł ${nodeId} (${getNodeName(node)}) ma nieprawidłowy poziom: ${level}, ustawiam 0`);
            level = 0;
            nodeLevels.set(nodeId, 0);
        }
        
        // Upewnij się, że level jest liczbą całkowitą
        level = Math.floor(Number(level));
        
        return {
            id: nodeId,
            label: wrapLabelInThreeLines(getNodeName(node)),
            title: '', // Usuwamy HTML z title - vis.js nie renderuje HTML
            group: getNodeTag(node),
            level: level, // ZAWSZE liczba - vis.js wymaga tego dla hierarchicznego layoutu
            color: getNodeColor(node),
            font: {
                color: '#1f2937',
                size: 42,
                face: 'Inter, system-ui, sans-serif',
                align: 'center'
            },
            shape: getNodeIsStart(node) ? 'star' : 'dot',
            size: getNodeIsStart(node) ? 90 : 60,
            borderWidth: 4,
            borderWidthSelected: 6
        };
    });
    
    // Sprawdź czy wszystkie węzły mają poziom jako liczbę
    const nodesWithInvalidLevel = visNodesData.filter(n => n.level === undefined || n.level === null || isNaN(n.level));
    if (nodesWithInvalidLevel.length > 0) {
        console.error('BŁĄD: Niektóre węzły mają nieprawidłowy poziom:', nodesWithInvalidLevel);
    }
    
    console.log('Vis nodes data:', visNodesData.map(n => ({ id: n.id, label: n.label, level: n.level })));
    
    const visNodes = new vis.DataSet(visNodesData);

    const visEdges = new vis.DataSet(validEdges.map(edge => ({
        id: getEdgeId(edge),
        from: getEdgeFrom(edge),
        to: getEdgeTo(edge),
        arrows: 'to',
        color: {
            color: '#64748b',
            highlight: '#3b82f6',
            hover: '#3b82f6'
        },
        width: 2,
        smooth: {
            type: 'cubicBezier',
            forceDirection: 'vertical',
            roundness: 0.4
        }
    })));

    // Opcje wizualizacji
    const options = {
        layout: {
            hierarchical: {
                enabled: true,
                direction: 'UD', // Up-Down
                sortMethod: 'directed',
                levelSeparation: 300,
                nodeSpacing: 400,
                treeSpacing: 500,
                shakeTowards: 'leaves'
            }
        },
        physics: {
            enabled: false // Wyłącz fizykę dla hierarchicznego layoutu
        },
        interaction: {
            hover: true,
            tooltipDelay: 200,
            zoomView: true,
            dragView: true,
            navigationButtons: true,
            keyboard: {
                enabled: true
            }
        },
        nodes: {
            font: {
                color: '#1f2937',
                size: 60,
                face: 'Inter, system-ui, sans-serif',
                align: 'center',
                multi: true
            },
            shadow: {
                enabled: true,
                color: 'rgba(0,0,0,0.2)',
                size: 10,
                x: 2,
                y: 2
            }
        },
        edges: {
            shadow: {
                enabled: true,
                color: 'rgba(0,0,0,0.1)',
                size: 5,
                x: 1,
                y: 1
            }
        }
    };

    // Utwórz niestandardowy tooltip
    const tooltip = createCustomTooltip(container);

    // Utwórz instancję grafu
    const network = new vis.Network(container, {
        nodes: visNodes,
        edges: visEdges
    }, options);

    // Obserwuj zmiany rozmiaru kontenera i aktualizuj graf
    // To jest szczególnie ważne, gdy kontener nie ma wymiarów podczas inicjalizacji
    const resizeObserver = new ResizeObserver((entries) => {
        for (const entry of entries) {
            const rect = entry.contentRect;
            if (rect.width > 0 && rect.height > 0) {
                // Upewnij się, że kontener ma prawidłową wysokość
                if (rect.height < 100) {
                    console.warn(`Container height is too small: ${rect.height}, setting minimum height`);
                    container.style.minHeight = '400px';
                }
                
                // Kontener ma wymiary - zaktualizuj rozmiar grafu
                network.setSize();
                
                // Dopasuj widok do zawartości
                setTimeout(() => {
                    network.fit({
                        animation: {
                            duration: 300,
                            easingFunction: 'easeInOutQuad'
                        }
                    });
                }, 100);
            }
        }
    });
    resizeObserver.observe(container);
    
    // Wymuś aktualizację rozmiaru natychmiast po utworzeniu
    setTimeout(() => {
        const rect = container.getBoundingClientRect();
        if (rect.height > 0) {
            network.setSize();
            console.log(`Forced network resize to: ${rect.width}x${rect.height}`);
        }
    }, 200);

    // Event handlers
    network.on('click', function (params) {
        if (params.nodes.length > 0) {
            // Kliknięto węzeł
            const nodeId = params.nodes[0];
            dotnetRef.invokeMethodAsync('HandleNodeClick', nodeId);
        } else {
            // Kliknięto tło
            dotnetRef.invokeMethodAsync('HandleBackgroundClick');
        }
    });

    let hoverTimeout;
    network.on('hoverNode', function (params) {
        container.style.cursor = 'pointer';
        
        // Pokaż tooltip z opóźnieniem
        clearTimeout(hoverTimeout);
        hoverTimeout = setTimeout(() => {
            const nodeId = params.node;
            const node = nodeDataMap.get(nodeId);
            if (node) {
                // Pobierz pozycję węzła w przestrzeni canvas
                const positions = network.getPositions([nodeId]);
                const canvasPosition = positions[nodeId];
                
                // Konwertuj pozycję canvas na współrzędne DOM
                const canvasBounds = container.getBoundingClientRect();
                const scale = network.getScale();
                const viewPosition = network.getViewPosition();
                
                // Oblicz pozycję węzła na ekranie
                const x = canvasBounds.left + (canvasPosition.x - viewPosition.x) * scale;
                const y = canvasBounds.top + (canvasPosition.y - viewPosition.y) * scale;
                
                showTooltip(tooltip, node, x, y);
            }
        }, 200);
    });

    network.on('blurNode', function () {
        container.style.cursor = 'default';
        clearTimeout(hoverTimeout);
        hideTooltip(tooltip);
    });

    // Zapisz instancję
    graphInstances.set(containerId, {
        network,
        nodes: visNodes,
        edges: visEdges,
        dotnetRef,
        nodeDataMap,
        tooltip,
        resizeObserver
    });

    // Wycentruj widok
    setTimeout(() => {
        network.fit({
            animation: {
                duration: 500,
                easingFunction: 'easeInOutQuad'
            }
        });
    }, 100);
}

/**
 * Aktualizuje graf nowymi danymi
 * @param {string} containerId - ID kontenera
 * @param {Array} nodes - Nowa tablica węzłów
 * @param {Array} edges - Nowa tablica krawędzi
 */
export function updateGraph(containerId, nodes, edges) {
    const instance = graphInstances.get(containerId);
    if (!instance) {
        console.error(`Graph instance ${containerId} not found`);
        return;
    }

    console.log('Updating graph:', { 
        containerId, 
        nodeCount: nodes?.length || 0, 
        edgeCount: edges?.length || 0 
    });

    // Walidacja danych
    if (!nodes || nodes.length === 0) {
        console.error('No nodes provided for update');
        return;
    }

    // Obsługa zarówno camelCase (id) jak i PascalCase (Id)
    const getNodeId = (node) => node.id ?? node.Id;
    const getNodeName = (node) => node.name ?? node.Name;
    const getNodeTag = (node) => node.tag ?? node.Tag;
    const getNodeIsStart = (node) => node.isStart ?? node.IsStart;
    const getNodeProgress = (node) => node.progress ?? node.Progress;
    const getEdgeId = (edge) => edge.id ?? edge.Id;
    const getEdgeFrom = (edge) => edge.from ?? edge.From;
    const getEdgeTo = (edge) => edge.to ?? edge.To;
    
    // Aktualizuj mapę danych węzłów
    instance.nodeDataMap.clear();
    const nodeIds = new Set();
    nodes.forEach(node => {
        const nodeId = getNodeId(node);
        instance.nodeDataMap.set(nodeId, node);
        nodeIds.add(nodeId);
    });

    // Waliduj krawędzie
    const validEdges = (edges || []).filter(edge => {
        const fromId = getEdgeFrom(edge);
        const toId = getEdgeTo(edge);
        if (!nodeIds.has(fromId)) {
            console.warn(`Edge ${getEdgeId(edge)} references non-existent node: ${fromId}`);
            return false;
        }
        if (!nodeIds.has(toId)) {
            console.warn(`Edge ${getEdgeId(edge)} references non-existent node: ${toId}`);
            return false;
        }
        return true;
    });

    // Oblicz poziomy dla węzłów
    const nodeLevels = calculateNodeLevels(nodes, validEdges, getNodeId, getNodeIsStart);
    
    // Upewnij się, że WSZYSTKIE węzły mają poziom (wymagane przez vis.js)
    nodes.forEach(node => {
        const nodeId = getNodeId(node);
        if (!nodeLevels.has(nodeId) || nodeLevels.get(nodeId) === undefined) {
            console.error(`BŁĄD w updateGraph: Węzeł ${nodeId} nie ma poziomu, ustawiam 0`);
            nodeLevels.set(nodeId, 0);
        }
    });

    // Przygotuj węzły - upewnij się, że wszystkie mają poziom jako LICZBĘ (nie undefined)
    const visNodesData = nodes.map(node => {
        const nodeId = getNodeId(node);
        let level = nodeLevels.get(nodeId);
        
        // Upewnij się, że level jest liczbą
        if (level === undefined || level === null || isNaN(level)) {
            console.error(`BŁĄD w updateGraph: Węzeł ${nodeId} (${getNodeName(node)}) ma nieprawidłowy poziom: ${level}, ustawiam 0`);
            level = 0;
            nodeLevels.set(nodeId, 0);
        }
        
        // Upewnij się, że level jest liczbą całkowitą
        level = Math.floor(Number(level));
        
        return {
            id: nodeId,
            label: wrapLabelInThreeLines(getNodeName(node)),
            title: '', // Usuwamy HTML z title
            group: getNodeTag(node),
            level: level, // ZAWSZE liczba - vis.js wymaga tego dla hierarchicznego layoutu
            color: getNodeColor(node),
            font: {
                color: '#1f2937',
                size: 42,
                face: 'Inter, system-ui, sans-serif',
                align: 'center'
            },
            shape: getNodeIsStart(node) ? 'star' : 'dot',
            size: getNodeIsStart(node) ? 90 : 60,
            borderWidth: 4,
            borderWidthSelected: 6
        };
    });
    
    const visNodes = visNodesData;

    // Aktualizuj krawędzie
    const visEdges = validEdges.map(edge => ({
        id: getEdgeId(edge),
        from: getEdgeFrom(edge),
        to: getEdgeTo(edge),
        arrows: 'to',
        color: {
            color: '#64748b',
            highlight: '#3b82f6',
            hover: '#3b82f6'
        },
        width: 2,
        smooth: {
            type: 'cubicBezier',
            forceDirection: 'vertical',
            roundness: 0.4
        }
    }));

    // Zaktualizuj DataSets
    instance.nodes.clear();
    instance.nodes.add(visNodes);
    instance.edges.clear();
    instance.edges.add(visEdges);

    // Animacja dopasowania widoku
    setTimeout(() => {
        instance.network.fit({
            animation: {
                duration: 500,
                easingFunction: 'easeInOutQuad'
            }
        });
    }, 100);
}

/**
 * Niszczy instancję grafu
 * @param {string} containerId - ID kontenera
 */
export function destroyGraph(containerId) {
    const instance = graphInstances.get(containerId);
    if (instance) {
        // Usuń tooltip jeśli istnieje
        if (instance.tooltip && instance.tooltip.parentNode) {
            instance.tooltip.parentNode.removeChild(instance.tooltip);
        }
        // Wyłącz ResizeObserver
        if (instance.resizeObserver) {
            instance.resizeObserver.disconnect();
        }
        instance.network.destroy();
        graphInstances.delete(containerId);
    }
}

/**
 * Tworzy niestandardowy element tooltip
 * @param {HTMLElement} container - Kontener grafu
 * @returns {HTMLElement} Element tooltip
 */
function createCustomTooltip(container) {
    const tooltip = document.createElement('div');
    tooltip.id = 'graph-tooltip';
    tooltip.style.cssText = `
        position: absolute;
        display: none;
        background: white;
        border: 1px solid #e5e7eb;
        border-radius: 8px;
        padding: 12px;
        box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06);
        z-index: 1000;
        pointer-events: none;
        max-width: 300px;
        font-family: Inter, system-ui, sans-serif;
    `;
    document.body.appendChild(tooltip);
    return tooltip;
}

/**
 * Pokazuje tooltip z danymi węzła
 * @param {HTMLElement} tooltip - Element tooltip
 * @param {object} node - Obiekt węzła
 * @param {number} x - Pozycja X na ekranie
 * @param {number} y - Pozycja Y na ekranie
 */
function showTooltip(tooltip, node, x, y) {
    // Obsługa zarówno camelCase jak i PascalCase
    const nodeName = node.name ?? node.Name ?? '';
    const nodePrefix = node.prefix ?? node.Prefix ?? '';
    const nodeTag = node.tag ?? node.Tag ?? '';
    const nodeProgress = node.progress ?? node.Progress ?? 0;
    const nodeSystemDescription = node.systemDescription ?? node.SystemDescription ?? '';
    
    const progressBar = `<div style="background: #e5e7eb; height: 8px; border-radius: 4px; margin-top: 8px; overflow: hidden;">
        <div style="background: ${getProgressColor(nodeProgress)}; height: 100%; width: ${nodeProgress}%; border-radius: 4px; transition: width 0.2s;"></div>
    </div>`;

    tooltip.innerHTML = `
        <div style="font-family: Inter, system-ui, sans-serif;">
            <strong style="font-size: 14px; color: #111827;">${escapeHtml(nodeName)}</strong><br/>
            <span style="color: #6b7280; font-size: 12px;">${escapeHtml(nodePrefix)} • ${escapeHtml(nodeTag)}</span><br/>
            <span style="font-size: 12px; color: #374151;">Postęp: ${nodeProgress}%</span>
            ${progressBar}
            ${nodeSystemDescription ? `<p style="margin-top: 8px; font-size: 12px; color: #4b5563; margin: 0;">${escapeHtml(nodeSystemDescription)}</p>` : ''}
        </div>
    `;
    
    tooltip.style.display = 'block';
    
    // Pozycjonuj tooltip obok węzła
    const tooltipRect = tooltip.getBoundingClientRect();
    const offsetX = 15;
    const offsetY = -tooltipRect.height / 2;
    
    tooltip.style.left = `${x + offsetX}px`;
    tooltip.style.top = `${y + offsetY}px`;
    
    // Sprawdź czy tooltip nie wychodzi poza ekran
    const windowWidth = window.innerWidth;
    const windowHeight = window.innerHeight;
    
    if (x + offsetX + tooltipRect.width > windowWidth) {
        tooltip.style.left = `${x - tooltipRect.width - offsetX}px`;
    }
    
    if (y + offsetY < 0) {
        tooltip.style.top = `${y + 15}px`;
    } else if (y + offsetY + tooltipRect.height > windowHeight) {
        tooltip.style.top = `${y - tooltipRect.height - 15}px`;
    }
}

/**
 * Ukrywa tooltip
 * @param {HTMLElement} tooltip - Element tooltip
 */
function hideTooltip(tooltip) {
    tooltip.style.display = 'none';
}

/**
 * Escapuje HTML aby zapobiec XSS
 * @param {string} text - Tekst do escapowania
 * @returns {string} Escapowany tekst
 */
function escapeHtml(text) {
    if (!text) return '';
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

/**
 * Zwraca kolor węzła na podstawie postępu
 * @param {object} node - Obiekt węzła
 * @returns {object} Obiekt kolorów vis.js
 */
function getNodeColor(node) {
    // Obsługa zarówno camelCase jak i PascalCase
    const isStart = node.isStart ?? node.IsStart ?? false;
    const progress = node.progress ?? node.Progress ?? 0;
    
    if (isStart) {
        return {
            background: '#f59e0b',
            border: '#d97706',
            highlight: {
                background: '#fbbf24',
                border: '#f59e0b'
            }
        };
    }
    let backgroundColor, borderColor;

    if (progress === 0) {
        // Nierozpoczęte - szary
        backgroundColor = '#9ca3af';
        borderColor = '#6b7280';
    } else if (progress < 30) {
        // Początkujący - czerwony
        backgroundColor = '#ef4444';
        borderColor = '#dc2626';
    } else if (progress < 70) {
        // Średniozaawansowany - żółty/pomarańczowy
        backgroundColor = '#f59e0b';
        borderColor = '#d97706';
    } else if (progress < 100) {
        // Zaawansowany - niebieski
        backgroundColor = '#3b82f6';
        borderColor = '#2563eb';
    } else {
        // Ukończone - zielony
        backgroundColor = '#10b981';
        borderColor = '#059669';
    }

    return {
        background: backgroundColor,
        border: borderColor,
        highlight: {
            background: backgroundColor,
            border: borderColor
        }
    };
}

/**
 * Zwraca kolor paska postępu
 * @param {number} progress - Postęp w procentach
 * @returns {string} Kolor hex
 */
function getProgressColor(progress) {
    if (progress === 0) return '#9ca3af';
    if (progress < 30) return '#ef4444';
    if (progress < 70) return '#f59e0b';
    if (progress < 100) return '#3b82f6';
    return '#10b981';
}
