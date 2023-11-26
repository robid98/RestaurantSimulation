const cacheName = 'v2'

self.addEventListener('install', () => { })

self.addEventListener('activate', (event) => {
    event.waitUntil(deleteOldCaches())
    self.registration.unregister()
        .then(function () {
            return self.clients.matchAll()
        })
        .then(function (clients) {
            clients.forEach(client => {
                client.navigate(client.url)
            })
        })
})

const deleteCache = async (key) => {
    await caches.delete(key)
}

const deleteOldCaches = async () => {
    const cacheKeepList = ["v2"]
    const keyList = await caches.keys()
    const cachesToDelete = keyList.filter((key) => !cacheKeepList.includes(key))
    await Promise.all(cachesToDelete.map(deleteCache))
}

const putInCache = async (request, response) => {
    const cache = await caches.open("v1")
    await cache.put(request, response)
}

const cacheFirst = async ({ request, preloadResponsePromise, fallbackUrl }) => {
    const responseFromCache = await caches.match(request)
    if (responseFromCache) return responseFromCache

    const preloadResponse = await preloadResponsePromise
    if (preloadResponse) {
        putInCache(request, preloadResponse.clone())
        return preloadResponse
    }

    try {

        const responseFromNetwork = await fetch(request)
        putInCache(request, responseFromNetwork.clone())
        return responseFromNetwork

    } catch (error) {

        const fallbackResponse = await caches.match(fallbackUrl)
        if (fallbackResponse) {
            return fallbackResponse
        }

        return new Response("Network error", {
            status: 408,
            headers: { "Content-Type": "text/plain" },
        })

    }

}

const fetchEvent = () => {
    self.addEventListener('fetch', (event) => {
        if (String(event.request.url).includes("/api/")) return event.respondWith(fetch(event.request))
        event.respondWith(cacheFirst({
            request: event.request,
            preloadResponsePromise: event.preloadResponse
        }))
    })
}

fetchEvent()