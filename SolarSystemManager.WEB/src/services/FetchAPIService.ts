export default class FetchAPIService {
  private static baseURL: string = 'https://localhost:7247'

  static get(url: string): any {
    console.log('Fetching data from: ' + this.baseURL + url)

    fetch(this.baseURL + url, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*'
      }
    })
      .then((response) => {
        if (!response.ok) {
          throw new Error('Network response was not ok.')
        }
        return response.json()
      })
      .catch((error) => {
        console.error('Error in FetchAPIService:', error)
      })
  }
}
