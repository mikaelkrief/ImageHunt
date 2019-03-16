@Pipe({ name: "sortByField" })
export class SortPipe implements PipeTransform {
  transform(items: any[], field: string, reverse: boolean): any[] {

    if (items) {
      let sortedItems = items.sort((a, b) => a[field] < b[field] ? -1 : (a[field] > b[field] ? 1 : 0));
      if (reverse) {
        sortedItems = sortedItems.slice().reverse();
      }
      return sortedItems;
    }
    return items;
  }
}
